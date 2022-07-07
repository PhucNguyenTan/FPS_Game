
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
    #endregion

    #region Components variables
    public Gun_Pistol Pistol;
    [SerializeField] private Camera fpsCam;
    public CharacterController pController { get; private set; }

    [SerializeField] private Player_data P_Data;
    [SerializeField] private Health_script Health_Data;
    #endregion

    #region Input variable
    public Vector2 mouseDelta { get; private set; }
    public Vector2 moveInput { get; private set; }
    #endregion

    #region Normal variables
    private bool isPause = false;
    public float _currentHeight { get; private set; }
    private float yRotation = 0f;
    private float _y_vcurrent = 0f;
    private float _x_vcurrent = 0f;
    private float _z_vcurrent = 0f;
    private float gravity;
    private float _friction;
    private float _initialJumpVelocity;
    private float _initialDashVelocity;
    private int _zDashDirection = 0;
    private int _xDashDirection = 0;
    public bool isDashing { get; private set; } = false;
    #endregion



    #region Unity Callbacks
    void Start()
    {
        
        movementMachine = new Player_state_machine();
        shootingMachine = new Player_state_machine();
        stateIdle = new Player_state_idle(this, movementMachine, "idle");
        stateMove = new Player_state_move(this, movementMachine, "move");
        stateJump = new Player_state_jump(this, movementMachine, "jump");

        stateDash = new Player_state_dash(this, movementMachine, "dash");
        stateCrouch = new Player_state_crouch(this, movementMachine, "crouch");
        //stateSlide = new Player_state_slide(this, movementMachine, "slide");

        stateShoot = new Player_state_shoot(this, shootingMachine, "shoot");
        stateGunidle = new Player_state_gunIdle(this, shootingMachine, "gun_idle");
        pController = GetComponent<CharacterController>();

        movementMachine.Initiallized(stateIdle);
        shootingMachine.Initiallized(stateGunidle);

        GameManager.OnChangeState += GameManagerOnChangeState;

        Cursor.lockState = CursorLockMode.Locked;

        SubscribeToInput();

        _currentHeight = P_Data.StandHeight;

        //InputHandler.pInputActrion.Gameplay.Crouch.performed +=;
        //InputHandler.pInputActrion.Gameplay.Dash.performed +=;
    }

    void Update()
    {
        if (!isPause)
        {
            moveInput = InputHandler.GetMoveInput();
            mouseDelta = InputHandler.GetMouseDelta();
            
            movementMachine.currentState.Logic();
            shootingMachine.currentState.Logic();

            PlayerRotate(mouseDelta);
            PlayerChanges();
            Pistol.ActuallyChangeDoChanges();
            //Debug.Log(_y_vcurrent);
            Debug.Log("Y: " + _z_vcurrent + " X:" + _x_vcurrent);
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
    #region Input turing off and on
    public void SubscribeToInput() {
        InputHandler.pInputActrion.Gameplay.Jump.performed += PlayerJump;
        InputHandler.pInputActrion.Gameplay.Dash.performed += PlayerDash;
        InputHandler.pInputActrion.Gameplay.Crouch.performed += PlayerCrouch;
    }

    public void UnsubcribeToInput()
    {
        InputHandler.pInputActrion.Gameplay.Jump.performed -= PlayerJump;
        InputHandler.pInputActrion.Gameplay.Dash.performed -= PlayerDash;
        InputHandler.pInputActrion.Gameplay.Crouch.performed -= PlayerCrouch;
    }

    public void SubscibeToShoot()
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
        _y_vcurrent = _initialJumpVelocity;
    }

    public void PlayerDash(InputAction.CallbackContext obj)
    {

        if (isDashing) return;
        if (Mathf.Abs(moveInput.x) < 0.5f && Mathf.Abs(moveInput.y) < 0.5f) return;
        isDashing = true;
        if(Mathf.Abs(moveInput.y) > 0.5f)
        {
            _zDashDirection = moveInput.y > 0f ? 1 : -1;
            _z_vcurrent =  _zDashDirection * P_Data.DashSpeed;
        }
        if(Mathf.Abs(moveInput.x) > 0.5f)
        {
            _xDashDirection = moveInput.x > 0f ? 1 : -1;
            _x_vcurrent = _xDashDirection * P_Data.DashSpeed;
        }
    }

    public void PlayerCrouch(InputAction.CallbackContext obj)
    {
        pController.height = P_Data.CrouchHeight;
        Debug.Log("crouch");
    }

    public void PlayerMove(Vector2 moveInput)
    {
        _z_vcurrent = moveInput.y * P_Data.MoveSpeed;
        _x_vcurrent = moveInput.x * P_Data.MoveSpeed;
    }

    public void AddFriction()
    {
        if (_xDashDirection == 1)
            _x_vcurrent -= P_Data.Friction;
        else if(_xDashDirection == -1)
            _x_vcurrent += P_Data.Friction;
        if (_zDashDirection == 1)
            _z_vcurrent -= P_Data.Friction;
        else if(_zDashDirection == -1)
            _z_vcurrent += P_Data.Friction;
    }

    public void PlayerChanges()
    {
        if(_y_vcurrent <= 0f)
        {
            SetJumpVar(P_Data.DropTime, P_Data.DropHeight);
        }
        Vector3 yVelocity = new Vector3(0f, _y_vcurrent + gravity*Time.deltaTime, 0f);
        Vector3 groundMove = (transform.right * _x_vcurrent + transform.forward * _z_vcurrent) * Time.deltaTime;
        pController.Move(yVelocity + groundMove);
    }

    private void PlayerRotate(Vector2 cameraInput)
    {
        yRotation -= cameraInput.y * P_Data.MouseSensitivity; // ???
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        fpsCam.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        transform.Rotate(Vector3.up * cameraInput.x * P_Data.MouseSensitivity);
    }

    
    #endregion
    #region Setup functions

    public void Grounded()
    {
        _y_vcurrent = P_Data.GroundGravity;
    }

    public void StopDash()
    {
        isDashing = false;
        _xDashDirection = 0;
        _zDashDirection = 0;
        _z_vcurrent = 0f;
        _x_vcurrent = 0f;
    }

    public void SetJumpVar(float maxJumpTime, float maxJumpHeight)
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / (timeToApex * timeToApex);
        _initialJumpVelocity =  (2 * maxJumpHeight) / timeToApex;
        
    }

    public void AddGravitry()
    {
        _y_vcurrent += gravity * Time.deltaTime;
        _y_vcurrent = Mathf.Max(_y_vcurrent, P_Data.EarthGravity);
    }
    #endregion

    #region Set Effect function
    public void TakeDamage(float damage)
    {
        Health_Data.DecreaseHealth(10f);
    }
    #endregion

    #region Check function
    public bool isInputingMove()
    {
        if(moveInput.x !=0 || moveInput.y != 0)
        {
            return true;
        }
        return false;
    }

    public bool Is_xDashStop()
    {
        if (_xDashDirection == 1)
            return _x_vcurrent < 0 ? true : false;
        else if(_xDashDirection == -1)
            return _x_vcurrent > 0 ? true : false;
        return true;
    }

    public bool Is_zDashStop()
    {
        if (_zDashDirection == 1)
            return _z_vcurrent < 0 ? true : false;
        else if(_zDashDirection == -1)
            return _z_vcurrent > 0 ? true : false;
        return true;
    }

    #endregion
    #region Get functions
    public Vector3 GetFPScamPosition()
    {
        return fpsCam.transform.position;
    }
    #endregion


}
