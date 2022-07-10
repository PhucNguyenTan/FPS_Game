using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_state_idle : Player_base_state
{
    public Player_state_idle(Player player, Player_state_machine stateMachine, Player_data playerData, string animString) : base(player, stateMachine, playerData, animString)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.Grounded();
        InputHandler.pInputActrion.Gameplay.Jump.performed += player.PlayerJump;
        player.Pistol.PlayAnim("pistol_idle");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Logic()
    {
        base.Logic();
        if (!player.pController.isGrounded)
        {
            stateMachine.ChangeStage(player.stateJump);
        }
        if(player.isInputingMove())
        {
            stateMachine.ChangeStage(player.stateMove);
        }
        if (player.isCrouching)
            stateMachine.ChangeStage(player.stateCrouch);


    }


}
