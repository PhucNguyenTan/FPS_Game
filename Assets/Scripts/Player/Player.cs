
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Variable for State
    public Player_state_machine movementMachine { get; private set; }
    public Player_state_machine shootingMachine { get; private set; }
    public Player_state_idle stateIdle { get; private set; }
    public Player_state_jump stateJump { get; private set; }
    public Player_state_move stateMove { get; private set; }
    public Player_state_shoot stateShoot { get; private set; }
    public Player_state_gunIdle stateGunidle { get; private set; }
    public Player_state_dash stateDash { get; private set; }
    public Player_state_crouch stateCrouch { get; private set; }
    public Player_state_slide stateSlide { get; private set; }
    public Player_state_wallClimb stateWallClimb { get; private set; }
    public Player_state_wallRun stateWallRun { get; private set; }
    #endregion

    #region Components variablesm
    public Gun_Pistol Pistol;
    private CapsuleCollider _col;
    [SerializeField] private Camera fpsCam;
    public CharacterController pController { get; private set; }

    [SerializeField] private Player_data P_Data;
    [SerializeField] private Health_script Health_Data;

    #endregion

    #region Input variable
    public Vector2 mouseDelta { get; private set; }
    public Vector2 moveInput { get; private set; }
    public Vector2 camInput { get; private set; }
    #endregion

    #region Normal variables
    private bool isPause = false;
    public float _currentHeight { get; private set; }
    private float yRotation = 0f;
    private float gravity;
    private float _friction;
    private float _initialJumpVelocity;

    private float _up_vcurrent = 0f;
    private float _side_vcurrent = 0f;
    private float _forward_vcurrent = 0f;
    private float _timeLapseCrouch = 0f;

    public float WallTouchedAngle { get; private set; } = 0f;
    public int _yDashDirection { get; private set; } = 0;
    public int _xDashDirection { get; private set; } = 0;
    private Vector3 _xDashVector;
    private Vector3 _yDashVector;
    public bool isDashing { get; private set; } = false;
    public bool isCrouching { get; private set; } = false;

    public float crouchLapse = 0f;

    private RaycastHit _currentWallHit;

    private Vector3 _forward_currentDir;
    private Vector3 _side_currentDir;

    private Vector3[] _findWallDirection;
    private RaycastHit[] _hitFindWall;
    public DashDirection DashDir { get; private set;} = DashDirection.None;
    private Vector3 _wallRunDirection;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        
        movementMachine = new Player_state_machine();
        shootingMachine = new Player_state_machine();
        stateIdle = new Player_state_idle(this, movementMachine, P_Data, "idle");
        stateMove = new Player_state_move(this, movementMachine, P_Data, "move");
        stateJump = new Player_state_jump(this, movementMachine, P_Data, "jump");

        stateDash = new Player_state_dash(this, movementMachine, P_Data, "dash");
        stateCrouch = new Player_state_crouch(this, movementMachine, P_Data, "crouch");
        stateSlide = new Player_state_slide(this, movementMachine, P_Data, "slide");
        stateWallClimb = new Player_state_wallClimb(this, movementMachine, P_Data, "wallClimb");
        stateWallRun = new Player_state_wallRun(this, movementMachine, P_Data, "wallRun");

        stateShoot = new Player_state_shoot(this, shootingMachine, P_Data, "shoot");
        stateGunidle = new Player_state_gunIdle(this, shootingMachine,P_Data, "gun_idle");
        pController = GetComponent<CharacterController>();
        _col = GetComponent<CapsuleCollider>();

        movementMachine.Initiallized(stateIdle);
        shootingMachine.Initiallized(stateGunidle);

        GameManager.OnChangeState += GameManagerOnChangeState;

        Cursor.lockState = CursorLockMode.Locked;

        SubscribeToMovementInput();

        _currentHeight = P_Data.StandHeight;

        //InputHandler.pInputActrion.Gameplay.Crouch.performed +=;
        //InputHandler.pInputActrion.Gameplay.Dash.performed +=;
        _findWallDirection = new Vector3[]{
            Vector3.forward,
            Vector3.right,
            Vector3.back,
            Vector3.left,
            Vector3.forward + Vector3.right,
            Vector3.forward + Vector3.left,
            Vector3.back    + Vector3.right,
            Vector3.back    + Vector3.left
            
        };
    }

    void Update()
    {
        if (!isPause)
        {
            moveInput = InputHandler.GetMoveInput();
            mouseDelta = InputHandler.GetMouseDelta() * P_Data.MouseSensitivity;
            camInput = InputHandler.GetCamInput() * P_Data.JoystickCamSpeed;

            movementMachine.currentState.Logic();
            shootingMachine.currentState.Logic();

            PlayerRotate(mouseDelta + camInput);
            PlayerApplyMovement();
            Pistol.ActuallyApplyDoChanges();
            AdjustHeight();
        }
    }
    #endregion
    #region Game Manager Listener
    private void GameManagerOnChangeState(GameManager.GameState arg0)
    {
        switch (GameManager.State)
        {
            case (GameManager.GameState.CountDown):
                break;
        }
    }
    #endregion
    #region Input turning off and on
    public void SubscribeToMovementInput() {
        InputHandler.pInputActrion.Gameplay.Jump.performed += PlayerJump;
        InputHandler.pInputActrion.Gameplay.Dash.performed += PlayerDash;
        InputHandler.pInputActrion.Gameplay.Crouch.performed += _ => isCrouching = true;
        InputHandler.pInputActrion.Gameplay.Crouch.canceled += _ => isCrouching = false;
    }

    public void UnsubcribeToMovementInput()
    {
        InputHandler.pInputActrion.Gameplay.Jump.performed -= PlayerJump;
        InputHandler.pInputActrion.Gameplay.Dash.performed -= PlayerDash;
        InputHandler.pInputActrion.Gameplay.Crouch.performed -= _ => isCrouching = true;
        InputHandler.pInputActrion.Gameplay.Crouch.canceled -= _ => isCrouching = false;
    }


    public void SubscribeToShoot()
    {
        InputHandler.pInputActrion.Gameplay.Shoot.performed += PlayerShoot;
    }

    public void UnsubscribeToShoot()
    {
        InputHandler.pInputActrion.Gameplay.Shoot.performed -= PlayerShoot;
    }

    #endregion
    #region Functions for subscribing
    private void PlayerShoot(InputAction.CallbackContext obj)
    {
        
        RaycastHit hit;
        Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit);
        Pistol.PistolShoot(hit);

    }

    public void PlayerJump(InputAction.CallbackContext obj)
    {
        SetJumpVar(P_Data.JumpTime, P_Data.JumpHeight);
        _up_vcurrent = _initialJumpVelocity;
    }
    
    public void PlayerDash(InputAction.CallbackContext obj)
    {

        if (isDashing) return;
        if (Mathf.Abs(moveInput.x) < 0.5f && Mathf.Abs(moveInput.y) < 0.5f) return;
        isDashing = true;
        if(Mathf.Abs(moveInput.y) > 0.5f)
        {
            _yDashDirection = moveInput.y > 0f ? 1 : -1;
            _forward_vcurrent =  _yDashDirection * P_Data.DashSpeed;
        }
        if(Mathf.Abs(moveInput.x) > 0.5f)
        {
            _xDashDirection = moveInput.x > 0f ? 1 : -1;
            _side_vcurrent = _xDashDirection * P_Data.DashSpeed;
        }
        _xDashVector = transform.right;
        _yDashVector = transform.forward;
    }

    public void PlayerCrouch(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            isCrouching = true;
        }
        else if (obj.canceled)
        {
            isCrouching = false;
        }
    }

    public void PlayerMove(Vector2 moveInput)
    {
        _forward_vcurrent = moveInput.y;
        _side_vcurrent = moveInput.x ;
        ApplyWalkDirection();
    }

    public void AddFriction(float frictionValue)
    {
        if (_xDashDirection == 1)
            _side_vcurrent -= frictionValue;
        else if(_xDashDirection == -1)
            _side_vcurrent += frictionValue;
        if (_yDashDirection == 1)
            _forward_vcurrent -= frictionValue;
        else if(_yDashDirection == -1)
            _forward_vcurrent += frictionValue;
    }

    public void PlayerApplyMovement()
    {
        if(_up_vcurrent <= 0f) // When player at peak jump, or when drop off a ledge
        {
            SetJumpVar(P_Data.DropTime, P_Data.DropHeight);
        }

        Vector3 groundMove = (_side_currentDir * _side_vcurrent + _forward_currentDir * _forward_vcurrent) * Time.deltaTime;
        Vector3 yVelocity = new Vector3(0f, _up_vcurrent + gravity * Time.deltaTime, 0f);
        pController.Move(yVelocity + groundMove);
    }

    private void PlayerRotate(Vector2 cameraInput)
    {
        yRotation -= cameraInput.y; // ???
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        fpsCam.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        transform.Rotate(Vector3.up * cameraInput.x);
    }

    public void PlayerWallClimbJump(InputAction.CallbackContext obj)
    {
        _forward_vcurrent += P_Data.Forward_WallJump;
        _side_vcurrent += P_Data.Side_WallJump;
        ApplyWallJumpDirection();

    }

    public void PlayerWallRunJump(InputAction.CallbackContext obj)
    {
        _forward_vcurrent += P_Data.Forward_WallJump;
        _side_vcurrent += P_Data.Side_WallJump;
        ApplyWallRunJumpDirection(); 
        SetJumpVar(P_Data.WallRunJumpTIme, P_Data.WallRunJumpHeight);
        SetUpVelocity();
    }
    #endregion
    #region Setup functions


    public void Grounded()
    {
        _up_vcurrent = P_Data.GroundGravity;
    }

    public void StopDash()
    {
        isDashing = false;
        _xDashDirection = 0;
        _yDashDirection = 0;
        _forward_vcurrent = 0f;
        _side_vcurrent = 0f;
    }

    public void SetJumpVar(float maxJumpTime, float maxJumpHeight)
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / (timeToApex * timeToApex);
        _initialJumpVelocity =  (2 * maxJumpHeight) / timeToApex;
        
    }

    public void AdjustHeight()
    {
        if (isCrouching)
        {
            if(crouchLapse == 1f) // Already at crouch height;
                return;
            crouchLapse += Time.deltaTime * P_Data.CrouchSpeed;
        }
        else
        {
            if(crouchLapse == 0f) // Already at stand height;
                return;
            crouchLapse -= Time.deltaTime * P_Data.CrouchSpeed;
        }
        crouchLapse = Mathf.Clamp(crouchLapse, 0f, 1f);

        Vector3 v3Stand = new Vector3(0f, P_Data.camStandHeight,0f);
        Vector3 v3Crouch = new Vector3(0f, P_Data.camCrouchHeight, 0f);

        pController.height = Mathf.Lerp(P_Data.StandHeight, P_Data.CrouchHeight, crouchLapse);
        pController.center = Vector3.Lerp(v3Stand, v3Crouch, crouchLapse);
        fpsCam.transform.localPosition = Vector3.Lerp(v3Stand, v3Crouch, crouchLapse);
        _col.center = pController.center;
        _col.height = pController.height;

    }

    public void ResetTouchAngle()
    {
        WallTouchedAngle = 0f;
    }

    public void AddGravity()
    {
        _up_vcurrent += gravity * Time.deltaTime;
        _up_vcurrent = Mathf.Max(_up_vcurrent, P_Data.EarthGravity);
    }

    public void SetUpVelocity()
    {
        _up_vcurrent = _initialJumpVelocity;
    }

    public void StopJumpvelocity()
    {
        _up_vcurrent = 0f;
    }

    public void ApplyMovementForce(float xForce, float yForce)
    {
        _forward_vcurrent = yForce;
        _side_vcurrent = xForce;
    }

    public void ApplyWallRunJumpDirection()
    {
        _side_currentDir = _currentWallHit.normal;
        _forward_currentDir = _wallRunDirection;
    }

    public void ApplyWallRunDirection()
    {
        _side_currentDir = Vector3.zero;
        _forward_currentDir = _wallRunDirection;
    }

    public void ApplyMomentumDirection()
    {
        _side_currentDir = _xDashVector;
        _forward_currentDir = _yDashVector;
    }

    public void ApplyWallJumpDirection()
    {
        _side_currentDir = _currentWallHit.normal;
        _forward_currentDir = _currentWallHit.normal;
    }

    public void ApplyWalkDirection()
    {
        _side_currentDir = transform.right;
        _forward_currentDir = transform.forward;
    }

    #endregion

    #region Set Effect function
    public void TakeDamage(float damage)
    {
        Health_Data.DecreaseHealth(10f);
    }
    #endregion

    #region Check function
    public bool IsInputingMove()
    {
        if(moveInput.x !=0f || moveInput.y != 0f)
        {
            return true;
        }
        return false;
    }

    public bool Is_xDashStop()
    {
        if (_xDashDirection == 1)
            return _side_vcurrent < 0 ? true : false;
        else if(_xDashDirection == -1)
            return _side_vcurrent > 0 ? true : false;
        return true;
    }

    public bool Is_zDashStop()
    {
        if (_yDashDirection == 1)
            return _forward_vcurrent < 0 ? true : false;
        else if(_yDashDirection == -1)
            return _forward_vcurrent > 0 ? true : false;
        return true;
    }


    #endregion
    #region Get functions
    public Vector3 GetFPScamPosition()
    {
        return fpsCam.transform.position;
    }

    public Vector2 GetDashPercentage()
    {
        Vector2 dashPercent;
        dashPercent.x = Mathf.Abs(_side_vcurrent) / P_Data.DashSpeed;
        dashPercent.y = Mathf.Abs(_forward_vcurrent) / P_Data.DashSpeed;
        return dashPercent;
    }
    #endregion

    #region Temp region
    public void CastRayWall()
    {
        bool isWallNear = false;
        if (_xDashDirection == 0 && _yDashDirection == 1) // Forward
        {
            isWallNear = Physics.Raycast(transform.position, transform.forward, out _currentWallHit, 1f, P_Data.Climable);
            WallTouchedAngle = Vector3.Angle(_currentWallHit.normal, transform.forward);
            DashDir = DashDirection.Forward;
        }
        else if(_xDashDirection == 1 && _yDashDirection == 0) // Right
        {
            isWallNear = Physics.Raycast(transform.position, transform.right, out _currentWallHit, 1f, P_Data.Climable);
            WallTouchedAngle = Vector3.Angle(_currentWallHit.normal, transform.right);
            DashDir = DashDirection.Right;
        }
        else if (_xDashDirection == -1 && _yDashDirection == 0) // Left
        {
            isWallNear = Physics.Raycast(transform.position, -transform.right, out _currentWallHit, 1f, P_Data.Climable);
            WallTouchedAngle = Vector3.Angle(_currentWallHit.normal, -transform.right);
            DashDir = DashDirection.Left;
        }
        else if (_xDashDirection == 0 && _yDashDirection == -1) // Backward
        {
            isWallNear = Physics.Raycast(transform.position, -transform.forward, out _currentWallHit, 1f, P_Data.Climable);
            WallTouchedAngle = Vector3.Angle(_currentWallHit.normal, -transform.forward);
            DashDir = DashDirection.Backward;
        }
        else if (_xDashDirection == 1 && _yDashDirection == 1) // Forward right
        {
            isWallNear = Physics.Raycast(transform.position, transform.right, out _currentWallHit, 1f, P_Data.Climable);
            WallTouchedAngle = Vector3.Angle(_currentWallHit.normal, transform.right);
            DashDir = DashDirection.Forward_Right;
        }
        else if (_xDashDirection == -1 && _yDashDirection == 1) // forwward left
        {
            isWallNear = Physics.Raycast(transform.position, -transform.right, out _currentWallHit, 1f, P_Data.Climable);
            WallTouchedAngle = Vector3.Angle(_currentWallHit.normal, -transform.right);
            DashDir = DashDirection.Forward_Left;
        }
    }

    public bool CheckIfObjectNear()
    {
        RaycastHit[] checkRays = new RaycastHit[_findWallDirection.Length];
        for (int i = 0; i < _findWallDirection.Length; i++){
            Vector3 dir = transform.TransformDirection(_findWallDirection[i]);
            bool hit = Physics.Raycast(transform.position, dir , out checkRays[i], 1f, P_Data.Climable);
            
            if(hit)
            {
                Debug.DrawRay(transform.position, dir * P_Data.RayCheckLength, Color.red, 1f, false);
                return true;
            }
        }
        return false;
    }

    public void StopGroundVelocity()
    {
        _forward_vcurrent = 0f;
        _side_vcurrent = 0f;
    }

    public void FindWallDirection()
    {
        Vector3 wallForward = Vector3.Cross(_currentWallHit.normal, transform.up);
        if ((transform.forward - wallForward).magnitude > (transform.forward - -wallForward).magnitude)
            wallForward = -wallForward;
        _wallRunDirection = wallForward;
    }
    

    #endregion

    #region enum definition
    public enum DashDirection
    {
        None,
        Forward,
        Backward,
        Left,
        Right,
        Forward_Left,
        Forward_Right,
        Backward_Left,
        Backward_Right
    }
    #endregion
}
