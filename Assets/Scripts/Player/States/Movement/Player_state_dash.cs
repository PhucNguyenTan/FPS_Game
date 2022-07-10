using UnityEngine;

public class Player_state_dash : Player_base_state
{

    public Player_state_dash(Player player, Player_state_machine stateMachine, Player_data playerData, string animString) : base(player, stateMachine, playerData, animString)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.UnsubcribeToInput();
    }

    public override void Exit()
    {
        base.Exit();
        player.SubscribeToInput();
    }

    public override void Logic()
    {
        base.Logic();

        if (player.Is_xDashStop() && player.Is_zDashStop())
        {
            player.StopDash();
        }
        if (!player.isDashing && player.isInputingMove())
        {
            stateMachine.ChangeStage(player.stateMove);
        }
        if (!player.isDashing)
        {
            stateMachine.ChangeStage(player.stateIdle);
        }
        if (player.isCrouching)
        {
            stateMachine.ChangeStage(player.stateSlide);
        }
        player.Pistol.DashSway(player.GetDashPercentage(), player._xDashDirection, player._yDashDirection);
        player.AddFriction(playerData.DashFriction);



    }
}
