using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_state_stun : Enemy_base_state
{
    public Enemy_state_stun(Enemy enemy, Enemy_state_machine stateMachine, string stringAnim) : base(enemy, stateMachine, stringAnim)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Logic()
    {
        base.Logic();
    }
}
