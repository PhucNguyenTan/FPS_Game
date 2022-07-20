using UnityEngine;

public class Player_state_dash : Player_base_state
{

    public Player_state_dash(Player player, Player_state_machine stateMachine, Player_data playerData, string animString) : base(player, stateMachine, playerData, animString)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.UnsubcribeToMovementInput();
    }

    public override void Exit()
    {
        base.Exit();
        player.SubscribeToMovementInput();
    }

    public override void Logic()
    {
        base.Logic();

        if (player.Is_xDashStop() && player.Is_zDashStop())
        {
            player.StopDash();
        }
        if (!player.isDashing && player.IsInputingMove())
        {
            stateMachine.ChangeStage(player.stateMove);
        }
        else if (!player.isDashing)
        {
            stateMachine.ChangeStage(player.stateIdle);
        }
        else if (player.isCrouching)
        {
            stateMachine.ChangeStage(player.stateSlide);
        }
        else if (!player.pController.isGrounded)
        {
            player.SetDropoffVelocity();
            stateMachine.ChangeStage(player.stateJump);
        }
        player.Pistol.DashSway(player.GetDashPercentage(), player._xDashDirection, player._yDashDirection);
        player.AddFriction(playerData.DashFriction);



    }
}
