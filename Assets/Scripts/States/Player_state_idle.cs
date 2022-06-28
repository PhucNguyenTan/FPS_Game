using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_state_idle : Player_base_state
{
    public Player_state_idle(Player player, Player_state_machine stateMachine, string animString) : base(player, stateMachine, animString)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.Grounded();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Logic()
    {
        base.Logic();
        if (!player.pControl.isGrounded)
        {
            stateMachine.ChangeStage(player.stateJump);
        }

        InputHandler.pInputActrion.Gameplay.Jump.performed += player.PlayerJump;
    }


}
