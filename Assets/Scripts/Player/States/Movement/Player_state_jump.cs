using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_state_jump : Player_base_state
{

    public Player_state_jump(Player player, Player_state_machine stateMachine, Player_data playerData, string animString) : base(player, stateMachine, playerData, animString)
    {
    }

    public override void Enter()
    {
        base.Enter();
        InputHandler.pInputActrion.Gameplay.Jump.performed -= player.PlayerJump;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Logic()
    {
        base.Logic();
        if(player.pController.isGrounded && player.isInputingMove())
        {
            player.Grounded();
            stateMachine.ChangeStage(player.stateMove);
        }
        if (player.pController.isGrounded)
        {
            player.Grounded();
            stateMachine.ChangeStage(player.stateIdle);
        }
        player.AddGravitry();

    }
}
