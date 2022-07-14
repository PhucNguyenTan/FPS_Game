using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_state_crouch : Player_base_state
{

    public Player_state_crouch(Player player, Player_state_machine stateMachine, Player_data playerData, string animString) : base(player, stateMachine, playerData, animString)
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
        if (!player.isCrouching)
        {
            if (player.pController.isGrounded)
            {
                if (player.IsInputingMove())
                    stateMachine.ChangeStage(player.stateMove);
                else
                    stateMachine.ChangeStage(player.stateIdle);
            }
            else
                stateMachine.ChangeStage(player.stateJump);
        }
        else if (!player.pController.isGrounded)
        {
            stateMachine.ChangeStage(player.stateJump);
        }
        player.Pistol.MovementBob(player.moveInput);
        player.Pistol.MovementSway(player.moveInput);
        player.PlayerMove(player.moveInput * playerData.CrouchMoveSpeed);

    }
}
