

public class Player_state_wallRun : Player_base_state
{
    public Player_state_wallRun(Player player, Player_state_machine stateMachine, Player_data playerData, string animString) : base(player, stateMachine, playerData, animString)
    {
    }

    public override void Enter()
    {
        base.Enter();
        InputHandler.Instance.pInputAction.Gameplay.Jump.performed -= player.PlayerJump;
        InputHandler.Instance.pInputAction.Gameplay.Jump.performed += player.PlayerWallRunJump;
        player.StopGroundVelocity();
        player.StopJumpvelocity();
        player.FindWallDirection();
        player.ApplyWallRunDirection();
        //player.ApplyMovementForce(10f, 10f);
        player.TurnOffGravity();
    }

    public override void Exit()
    {
        base.Exit();
        InputHandler.Instance.pInputAction.Gameplay.Jump.performed -= player.PlayerWallRunJump;
        player.SetJumpVar(playerData.DropTime, playerData.DropHeight);
        player.ResetTouchAngle();
        player.TurnOnGravity();
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
