

public class Player_state_wallRun : Player_base_state
{
    public Player_state_wallRun(Player player, Player_state_machine stateMachine, Player_data playerData, string animString) : base(player, stateMachine, playerData, animString)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.StopGroundVelocity();
        InputHandler.pInputActrion.Gameplay.Jump.performed -= player.PlayerJump;
        InputHandler.pInputActrion.Gameplay.Jump.performed += player.PlayerWallClimbJump;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Logic()
    {
        base.Logic();
    }
}
