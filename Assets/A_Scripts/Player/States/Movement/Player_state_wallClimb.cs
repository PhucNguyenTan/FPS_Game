

public class Player_state_wallClimb : Player_base_state
{
    public Player_state_wallClimb(Player player, Player_state_machine stateMachine, Player_data playerData, string animString) : base(player, stateMachine, playerData, animString)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.StopGroundVelocity();
        InputHandler.Instance.pInputAction.Gameplay.Jump.performed -= player.PlayerJump;
        InputHandler.Instance.pInputAction.Gameplay.Jump.performed += player.PlayerWallClimbJump;
        player.SetJumpVar(playerData.ClimbTime, playerData.ClimbHeight);
        player.SetUpVelocity();
        
    }


    public override void Exit()
    {
        base.Exit();
        InputHandler.Instance.pInputAction.Gameplay.Jump.performed -= player.PlayerWallClimbJump;
        player.SetJumpVar(playerData.DropTime, playerData.DropHeight);
        player.ResetTouchAngle();
    }

    public override void Logic()
    {
        base.Logic();
        if (!player.CheckIfObjectNear())
        {
            if (player.pController.isGrounded)
                stateMachine.ChangeStage(player.stateIdle);
            else if (!player.pController.isGrounded)
                stateMachine.ChangeStage(player.stateJump);
        }
    }
}
