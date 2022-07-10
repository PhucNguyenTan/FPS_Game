using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_state_move : Player_base_state
{
    public Player_state_move(Player player, Player_state_machine stateMachine, Player_data playerData, string animString) : base(player, stateMachine, playerData, animString)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SubscribeToInput();
    }

    public override void Exit()
    {
        base.Exit();
        player.Pistol.ResetPosition();

    }

    public override void Logic()
    {
        base.Logic();
        if (!player.pController.isGrounded)
        {
            stateMachine.ChangeStage(player.stateJump);
        }
        if (!player.isInputingMove())
        {
            stateMachine.ChangeStage(player.stateIdle);
        }
        if (player.isDashing)
        {
            stateMachine.ChangeStage(player.stateDash);
        }
        if (player.isCrouching)
        {
            stateMachine.ChangeStage(player.stateCrouch);
        }
        else if (!player.isDashing && !player.isCrouching)
        {
            player.Pistol.MovementBob(player.moveInput);
            player.Pistol.MovementSway(player.moveInput);
            player.PlayerMove(player.moveInput * playerData.MoveSpeed);
        }
    }
}