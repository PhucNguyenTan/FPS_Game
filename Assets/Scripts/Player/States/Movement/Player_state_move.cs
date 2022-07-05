using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_state_move : Player_base_state
{
    public Player_state_move(Player player, Player_state_machine stateMachine, string animString) : base(player, stateMachine, animString)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        player.Pistol.ResetPosition();

    }

    public override void Logic()
    {
        base.Logic();
        if (!player.pControl.isGrounded)
        {
            stateMachine.ChangeStage(player.stateJump);
        }
        if (!player.isInputingMove())
        {
            stateMachine.ChangeStage(player.stateIdle);
        }
        player.Pistol.MovementBob(player.moveInput);
        player.Pistol.MovementSway(player.moveInput);

    }
}