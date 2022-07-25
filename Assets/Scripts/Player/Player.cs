
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

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
    public  Player_state_wallClimb stateWallClimb { get; private set; }
    public Player_state_wallRun stateWallRun { get; private set; }
    #endregion

    #region Components variablesm
    public Gun_Pistol Pistol;
    public Gun_Shotgun Shotgun;
    public Gun_Smg SMG;
    public Gun_Rocket Rocket;

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

    private float _vertical_vcurrent = 0f;
    private float _horizontal_vcurrent = 0f;
    private float _timeLapseCrouch = 0f;

    private Vector3 _dashDirection;
    private Vector3 _currentDirection;
    private Vector3 _wallRunDirection;

    public float WallTouchedAngle { get; private set; } = 0f;
    
    public bool isDashing { get; private set; } = false;
    public bool isCrouching { get; private set; } = false;

    public float crouchLapse = 0f;

    private RaycastHit _currentWallHit;

    private Vector3[] _findWallDirection;
    private RaycastHit[] _hitFindWall;

    private bool _isGravitySuspend = false;
    private bool _isJumpUp = false;

    

    public Gun_base CurrentGun { get; private set; }

    bool _isAutoShoot = false;
    bool _isShooting = false;
    bool _isPrevShoot = false;
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
        CurrentGun = SMG;
        _isAutoShoot = true;

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


            AdjustHeight();
            PlayerRotate(mouseDelta + camInput);
            PlayerApplyMovement();
            CurrentGun.ActuallyApplyDoChanges();
            UpdatePlayerShoot();
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
        InputHandler.pInputActrion.Gameplay.Shoot.performed += ctx => _isShooting = true;
        InputHandler.pInputActrion.Gameplay.Shoot.canceled += ctx => _isShooting = false;
    }

    public void UnsubscribeToShoot()
    {
        InputHandler.pInputActrion.Gameplay.Shoot.performed -= ctx => _isShooting = true;
        InputHandler.pInputActrion.Gameplay.Shoot.canceled -= ctx => _isShooting = false;
    }

    public void UpdatePlayerShoot()
    {
        if (!_isShooting) {
            _isPrevShoot = false;  
            return;
        }
        if (_isAutoShoot)
        {
            CurrentGun.CheckCanShoot(fpsCam.transform);
        }
        if (!_isPrevShoot)
        {
            CurrentGun.CheckCanShoot(fpsCam.transform);
            _isPrevShoot = true;
        }
    }

    #endregion
    #region Functions for subscribing

    public void PlayerJump(InputAction.CallbackContext obj)
    {
        SetJumpVar(P_Data.JumpTime, P_Data.JumpHeight);
        _vertical_vcurrent = _initialJumpVelocity;
        _isJumpUp = true;
    }
    
    public void PlayerDash(InputAction.CallbackContext obj)
    {

        if (isDashing) return;
        if (Mathf.Abs(moveInput.x) < 0.5f && Mathf.Abs(moveInput.y) < 0.5f) return;
        isDashing = true;
        SoundManager.Instance.PlayEffectOnce(P_Data.DashSound);
        _dashDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        _dashDirection = _dashDirection.normalized;
        _horizontal_vcurrent = P_Data.DashSpeed;
        _currentDirection = transform.TransformDirection(_dashDirection);
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
        _horizontal_vcurrent = 1f;
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
        _currentDirection = transform.TransformDirection(move);
        //_currentDirection = transform.forward.normalized * moveInput.y + transform.right.normalized * moveInput.x;
    }

    public void AddFriction(float frictionValue)
    {
        _horizontal_vcurrent = Mathf.MoveTowards(_horizontal_vcurrent, 0f, frictionValue);
    }

    public void PlayerApplyMovement()
    {
        if (!_isGravitySuspend)
            AddGravity();
        if (_vertical_vcurrent <= 0f) // When player at peak jump, or when drop off a ledge
        {
            SetJumpVar(P_Data.DropTime, P_Data.DropHeight);
            _isJumpUp = false;
        }
        Vector3 move = _currentDirection * _horizontal_vcurrent * Time.deltaTime;
        move.y = _vertical_vcurrent;
        pController.Move(move);
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
        _horizontal_vcurrent += P_Data.Forward_WallJump;
        ApplyWallJumpDirection();

    }

    public void PlayerWallRunJump(InputAction.CallbackContext obj)
    {
        _horizontal_vcurrent += P_Data.Forward_WallJump;
        ApplyWallRunJumpDirection(); 
        SetJumpVar(P_Data.WallRunJumpTIme, P_Data.WallRunJumpHeight);
        SetUpVelocity();
    }
    #endregion
    #region Setup functions

    //void SetDirection(Vector2 forwardDirection)
    //{
    //    _currentDirection = new Vector3(forwardDirection.x, 0f, forwardDirection.y);
    //}

    public void TurnOffGravity()
    {
        _isGravitySuspend = true;
    }

    public void TurnOnGravity()
    {
        _isGravitySuspend = false;
    }

    public void Grounded()
    {
        _vertical_vcurrent = P_Data.GroundGravity;
    }

    public void SetDropoffVelocity()
    {
        if(!_isJumpUp)
            _vertical_vcurrent = 0f;
    }

    public void StopDash()
    {
        isDashing = false;
        _horizontal_vcurrent = 0f;
        _dashDirection = Vector3.zero;
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
        //_vertical_vcurrent = Mathf.MoveTowards(_vertical_vcurrent, P_Data.EarthGravity, gravity * Time.deltaTime);
        _vertical_vcurrent += gravity * Time.deltaTime;
        _vertical_vcurrent = Mathf.Max(_vertical_vcurrent, P_Data.EarthGravity);
    }

    public void SetUpVelocity()
    {
        _vertical_vcurrent = _initialJumpVelocity;
    }

    public void StopJumpvelocity()
    {
        _vertical_vcurrent = 0f;
    }

    public void ApplyMovementForce(float force)
    {
        _horizontal_vcurrent = force;
    }

    public void ApplyWallRunJumpDirection()
    {
        _currentDirection = Vector3.Lerp(_wallRunDirection, _currentWallHit.normal, 0.3f);
    }

    public void ApplyWallRunDirection()
    {
        _currentDirection = _wallRunDirection;
    }

    public void ApplyMomentumDirection()
    {
        _currentDirection = _dashDirection;
    }

    public void ApplyWallJumpDirection()
    {
        _currentDirection = _currentWallHit.normal;
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

    public bool IsDashStop()
    {
        if (_horizontal_vcurrent == 0)
            return true;
        return false;
    }

    public bool CheckIfObjectNear()
    {
        RaycastHit[] checkRays = new RaycastHit[_findWallDirection.Length];
        for (int i = 0; i < _findWallDirection.Length; i++)
        {
            Vector3 dir = transform.TransformDirection(_findWallDirection[i]);
            bool hit = Physics.Raycast(transform.position, dir, out checkRays[i], 1f, P_Data.Climable);

            if (hit)
            {
                Debug.DrawRay(transform.position, dir * P_Data.RayCheckLength, Color.red, 1f, false);
                return true;
            }
        }
        return false;
    }
    #endregion
    #region Get functions
    public Vector3 GetFPScamPosition()
    {
        return fpsCam.transform.position;
    }

    public float GetDashPercentage()
    {
        return _horizontal_vcurrent/P_Data.DashSpeed;
    }
    #endregion

    #region Temp region
    //public void CastRayWall()
    //{
    //    bool isWallNear = false;
    //    if (_dashDirection == Vector3.forward) // Forward
    //    {
    //        isWallNear = Physics.Raycast(transform.position, transform.forward, out _currentWallHit, 1f, P_Data.Climable);
    //        WallTouchedAngle = Vector3.Angle(_currentWallHit.normal, transform.forward);
    //        DashDir = DashDirection.Forward;
    //    }
    //    else if(_xDashDirection == 1 && _yDashDirection == 0) // Right
    //    {
    //        isWallNear = Physics.Raycast(transform.position, transform.right, out _currentWallHit, 1f, P_Data.Climable);
    //        WallTouchedAngle = Vector3.Angle(_currentWallHit.normal, transform.right);
    //        DashDir = DashDirection.Right;
    //    }
    //    else if (_xDashDirection == -1 && _yDashDirection == 0) // Left
    //    {
    //        isWallNear = Physics.Raycast(transform.position, -transform.right, out _currentWallHit, 1f, P_Data.Climable);
    //        WallTouchedAngle = Vector3.Angle(_currentWallHit.normal, -transform.right);
    //        DashDir = DashDirection.Left;
    //    }
    //    else if (_xDashDirection == 0 && _yDashDirection == -1) // Backward
    //    {
    //        isWallNear = Physics.Raycast(transform.position, -transform.forward, out _currentWallHit, 1f, P_Data.Climable);
    //        WallTouchedAngle = Vector3.Angle(_currentWallHit.normal, -transform.forward);
    //        DashDir = DashDirection.Backward;
    //    }
    //    else if (_xDashDirection == 1 && _yDashDirection == 1) // Forward right
    //    {
    //        isWallNear = Physics.Raycast(transform.position, transform.right, out _currentWallHit, 1f, P_Data.Climable);
    //        WallTouchedAngle = Vector3.Angle(_currentWallHit.normal, transform.right);
    //        DashDir = DashDirection.Forward_Right;
    //    }
    //    else if (_xDashDirection == -1 && _yDashDirection == 1) // forwward left
    //    {
    //        isWallNear = Physics.Raycast(transform.position, -transform.right, out _currentWallHit, 1f, P_Data.Climable);
    //        WallTouchedAngle = Vector3.Angle(_currentWallHit.normal, -transform.right);
    //        DashDir = DashDirection.Forward_Left;
    //    }
    //}

    

    public void StopGroundVelocity()
    {
        _horizontal_vcurrent = 0f;
    }

    public void FindWallDirection()
    {
        Vector3 wallForward = Vector3.Cross(_currentWallHit.normal, transform.up);
        if ((transform.forward - wallForward).magnitude > (transform.forward - -wallForward).magnitude)
            wallForward = -wallForward;
        _wallRunDirection = wallForward;
    }
    

    #endregion
}
