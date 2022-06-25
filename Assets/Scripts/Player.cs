using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Player_state_idle stateIdle { get; private set; }
    public Player_state_jump stateJump { get; private set; }
    public Player_state_move stateMove { get; private set; }

    public Player_state_machine stateMachine;
    
    void Start()
    {
        stateMachine = new Player_state_machine();
        stateIdle = new Player_state_idle(this, stateMachine, "idle");
        stateMove = new Player_state_move(this, stateMachine, "move");
        stateJump = new Player_state_jump(this, stateMachine, "jump");

        stateMachine.Initiallized(stateIdle);
    }

    
    void Update()
    {
        stateMachine.currentState.Logic();
    }
}
