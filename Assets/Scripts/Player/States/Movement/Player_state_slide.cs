

public class Player_state_slide : Player_base_state
{
    public Player_state_slide(Player player, Player_state_machine stateMachine, Player_data playerData, string animString) : base(player, stateMachine, playerData, animString)
    {
    }

    public override void Enter()
    {
        base.Enter();
        InputHandler.Instance.pInputAction.Gameplay.Jump.performed += player.PlayerJump;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Logic()
    {
        base.Logic();
        if (player.IsDashStop())
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
        else if (!player.pController.isGrounded)
        {
            player.SetDropoffVelocity();
            stateMachine.ChangeStage(player.stateJump);
        }
        else if (player.isDashing && !player.isCrouching)
        {
            stateMachine.ChangeStage(player.stateDash);
        }
        
        //player.Pistol.DashSway(player.GetDashPercentage(), player._xDashDirection, player._yDashDirection);
        player.AddFriction(playerData.SlideFriction);
    }
}
