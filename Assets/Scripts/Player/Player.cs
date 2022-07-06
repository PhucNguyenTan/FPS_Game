
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Variable for State
    public Player_state_idle stateIdle { get; private set; }
    public Player_state_jump stateJump { get; private set; }
    public Player_state_move stateMove { get; private set; }
    public Player_state_shoot stateShoot { get; private set; }
    public Player_state_gunIdle stateGunidle { get; private set; }
    public Player_state_machine movementMachine { get; private set; }
    public Player_state_machine shootingMachine { get; private set; }
    #endregion

    #region Components variables
    public Gun_Pistol Pistol;
    [SerializeField] private HUD hudUI;
    [SerializeField] private Camera fpsCam;
    public CharacterController pController { get; private set; }

    [SerializeField] private Player_data P_Data;
    #endregion

    #region Input variable
    public Vector2 mouseDelta { get; private set; }
    public Vector2 moveInput { get; private set; }
    #endregion

    #region Normal variables
    private bool isPause = false;
    public float _currentHeight { get; private set; }
    private float yRotation = 0f;
    private float v_current = 0f;
    private float gravity;
    private float initialJumpVelocity;
    public float Health { get; private set; }
    #endregion



    #region Unity Flow
    void Start()
    {
        
        movementMachine = new Player_state_machine();
        shootingMachine = new Player_state_machine();
        stateIdle = new Player_state_idle(this, movementMachine, "idle");
        stateMove = new Player_state_move(this, movementMachine, "move");
        stateJump = new Player_state_jump(this, movementMachine, "jump");
        stateShoot = new Player_state_shoot(this, shootingMachine, "shoot");
        stateGunidle = new Player_state_gunIdle(this, shootingMachine, "gun_idle");
        pController = GetComponent<CharacterController>();
        //InputHandler.pInputActrion.Gameplay.Jump.performed += ;

        movementMachine.Initiallized(stateIdle);
        shootingMachine.Initiallized(stateGunidle);

        GameManager.OnChangeState += GameManagerOnChangeState;

        Cursor.lockState = CursorLockMode.Locked;

        SetJumpVar();

        SubscribeToInput();

        Health = P_Data.MaxHealth;

        hudUI.UpdateHealthBar(Health);

        _currentHeight = P_Data.StandHeight;

        //InputHandler.pInputActrion.Gameplay.Crouch.performed +=;
        //InputHandler.pInputActrion.Gameplay.Dash.performed +=;
    }

    void Update()
    {
        if (!isPause)
        {
            SetJumpVar();
            moveInput = InputHandler.GetMoveInput();
            mouseDelta = InputHandler.GetMouseDelta();

            
            movementMachine.currentState.Logic();
            shootingMachine.currentState.Logic();

            PlayerRotate(mouseDelta);
            PlayerMove(moveInput);
            Pistol.ActuallyChangeDoChanges();
            Debug.Log(v_current);
        }
    }
    #endregion
    #region Input turing off and on
    private void GameManagerOnChangeState(GameManager.GameState arg0)
    {
        switch (GameManager.State)
        {
            case (GameManager.GameState.CountDown):
                break;
        }
    }

    private void SubscribeToInput() {
        InputHandler.pInputActrion.Gameplay.Jump.performed += PlayerJump;
        InputHandler.pInputActrion.Gameplay.Dash.performed += PlayerDash;
        InputHandler.pInputActrion.Gameplay.Crouch.performed += PlayerCrouch;
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
        v_current = initialJumpVelocity;
        Debug.Log(v_current);
    }

    public void PlayerMove(Vector2 moveInput)
    {
        Vector3 V_gravity = new Vector3(0f, v_current, 0f);
        Vector3 groundMove = (transform.right * moveInput.x + transform.forward * moveInput.y) * P_Data.MoveSpeed * Time.deltaTime;
        pController.Move(V_gravity+groundMove);
    }

    private void PlayerRotate(Vector2 cameraInput)
    {
        yRotation -= cameraInput.y * P_Data.MouseSensitivity; // ???
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        fpsCam.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        transform.Rotate(Vector3.up * cameraInput.x * P_Data.MouseSensitivity);
    }

    public void PlayerDash(InputAction.CallbackContext obj)
    {

    }

    public void PlayerCrouch(InputAction.CallbackContext obj)
    {
        pController.height = P_Data.CrouchHeight;
        Debug.Log("crouch");
    }
    #endregion
    #region Setup functions
    private void Initialized()
    {
        
    }

    public void Grounded()
    {
        v_current = P_Data.GroundGravity;
    }

    public void SetJumpVar()
    {
        float timeToApex = P_Data.MaxJumpTime / 2;
        gravity = (-2 * P_Data.MaxJumpHeight) / timeToApex * timeToApex;
        initialJumpVelocity =  (2 * P_Data.MaxJumpHeight) / timeToApex;
    }

    public void AddGravitry()
    {
        v_current += P_Data.EarthGravity * Time.deltaTime;
        v_current = Mathf.Max(v_current, P_Data.EarthGravity);
    }
    #endregion

    #region Set Effect function
    public void TakeDamage(float damage)
    {
        Health -= damage;
        hudUI.UpdateHealthBar(Health);
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
    #endregion
    #region Get functions
    public Vector3 GetFPScamPosition()
    {
        return fpsCam.transform.position;
    }
    #endregion


}
