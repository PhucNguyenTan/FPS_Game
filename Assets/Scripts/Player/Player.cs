
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Player_state_idle stateIdle { get; private set; }
    public Player_state_jump stateJump { get; private set; }
    public Player_state_move stateMove { get; private set; }
    public Player_state_shoot stateShoot { get; private set; }
    public Player_state_gunIdle stateGunidle { get; private set; }

    public Player_state_machine movementMachine;
    public Player_state_machine shootingMachine;

    #region To put in GameDATA object
    public float mouseSensitivity = 0.3f;
    public float moveSpeed = 5f;
    #endregion

    [SerializeField]
    private HUD hudUI;

    [SerializeField]
    private Camera fpsCam;
    [SerializeField]
    private bool isPause = false;
    private Vector2 worldPos;

    private float yRotation = 0f;

    private float groundGravity = -0.5f;
    private float earthGravity = -9.8f;
    private float v_current = 0f;
    private float gravity;

    public float maxJumpTime = 2f;
    public float maxJumpHeight = 1f;
    private float initialJumpVelocity;

    public Gun_Pistol Pistol;
    public Vector2 moveInput { get; private set; }

    public CharacterController pControl { get; private set; }

    public float health { get; private set; } = 50f;

    public Vector2 mouseDelta { get; private set; }
    void Start()
    {
        movementMachine = new Player_state_machine();
        shootingMachine = new Player_state_machine();
        stateIdle = new Player_state_idle(this, movementMachine, "idle");
        stateMove = new Player_state_move(this, movementMachine, "move");
        stateJump = new Player_state_jump(this, movementMachine, "jump");
        stateShoot = new Player_state_shoot(this, shootingMachine, "shoot");
        stateGunidle = new Player_state_gunIdle(this, shootingMachine, "gun_idle");
        //InputHandler.pInputActrion.Gameplay.Jump.performed += ;

        pControl = GetComponent<CharacterController>();

        //fpsCam = transform.F("Main Camera").gameObject.GetComponent<Camera>();

        movementMachine.Initiallized(stateIdle);
        shootingMachine.Initiallized(stateGunidle);

        GameManager.OnChangeState += GameManagerOnChangeState;

        Cursor.lockState = CursorLockMode.Locked;

        SetJumpVar();

        SubscribeToInput();

        hudUI.UpdateHealthBar(health);

        //InputHandler.pInputActrion.Gameplay.Crouch.performed +=;
        //InputHandler.pInputActrion.Gameplay.Dash.performed +=;
    }

    private void GameManagerOnChangeState(GameManager.GameState arg0)
    {
        switch (GameManager.State)
        {
            case (GameManager.GameState.CountDown):
                break;
        }
    }

    void Update()
    {
        if (!isPause) {
            moveInput = InputHandler.GetMoveInput();
            mouseDelta = InputHandler.GetMouseDelta();

            
            movementMachine.currentState.Logic();
            shootingMachine.currentState.Logic();

            PlayerRotate(mouseDelta);
            PlayerMove(moveInput);
            Pistol.ActuallyChangeDoChanges();
        }
    }

    private void SubscribeToInput() {
        InputHandler.pInputActrion.Gameplay.Jump.performed += PlayerJump;
    }

    public void SubscibeToShoot()
    {
        InputHandler.pInputActrion.Gameplay.Shoot.performed += PlayerShoot;
    }

    public void UnsubscribeToShoot()
    {
        InputHandler.pInputActrion.Gameplay.Shoot.performed -= PlayerShoot;
    }



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
        Vector3 groundMove = (transform.right * moveInput.x + transform.forward * moveInput.y) * moveSpeed * Time.deltaTime;
        pControl.Move(V_gravity+groundMove);
    }

    

    private void PlayerRotate(Vector2 cameraInput)
    {
        yRotation -= cameraInput.y * mouseSensitivity; // ???
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        fpsCam.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        transform.Rotate(Vector3.up * cameraInput.x * mouseSensitivity);
    }

    

    private void Initialized()
    {
        
    }

    public void Grounded()
    {
        v_current = groundGravity;
    }

    public void AddGravitry()
    {
        v_current += gravity*Time.deltaTime;
        v_current = Mathf.Max(v_current, gravity);
    }

    public void SetJumpVar()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / timeToApex * timeToApex;
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

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


    

    public void TakeDamage(float damage) {
        health -= damage;
        hudUI.UpdateHealthBar(health);
    }

    public Vector3 GetFPScamPosition()
    {
        return fpsCam.transform.position;
    }

    public void Dash()
    {

    }

    public void Crouch()
    {

    }
}
