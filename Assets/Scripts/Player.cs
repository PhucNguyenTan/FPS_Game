
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Player_state_idle stateIdle { get; private set; }
    public Player_state_jump stateJump { get; private set; }
    public Player_state_move stateMove { get; private set; }

    public Player_state_machine stateMachine;

    #region To put in GameDATA object
    public float mouseSensitivity = 0.3f;
    public float moveSpeed = 5f;
    #endregion

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

    public CharacterController pControl { get; private set; }

    void Start()
    {
        stateMachine = new Player_state_machine();
        stateIdle = new Player_state_idle(this, stateMachine, "idle");
        stateMove = new Player_state_move(this, stateMachine, "move");
        stateJump = new Player_state_jump(this, stateMachine, "jump");
        //InputHandler.pInputActrion.Gameplay.Jump.performed += ;

        pControl = GetComponent<CharacterController>();

        //fpsCam = transform.F("Main Camera").gameObject.GetComponent<Camera>();

        stateMachine.Initiallized(stateIdle);

        GameManager.OnChangeState += GameManagerOnChangeState;

        Cursor.lockState = CursorLockMode.Locked;

        SetJumpVar();

        SubscribeToInput();
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
            stateMachine.currentState.Logic();
            Vector2 moveInput = InputHandler.GetMoveInput();
            Vector2 mouseDelta = InputHandler.GetMouseDelta();

            PlayerRotate(mouseDelta);
            PlayerMove(moveInput);
        }
    }

    private void SubscribeToInput() {
        InputHandler.pInputActrion.Gameplay.Jump.performed += PlayerJump;
        InputHandler.pInputActrion.Gameplay.Shoot.performed += PlayerShoot;
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



    private void PlayerMove(Vector2 moveInput)
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
    }

    public void SetJumpVar()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / timeToApex * timeToApex;
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }
}
