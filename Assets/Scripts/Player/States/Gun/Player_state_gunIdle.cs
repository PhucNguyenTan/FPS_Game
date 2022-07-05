

public class Player_state_gunIdle : Player_base_state
{
    public Player_state_gunIdle(Player player, Player_state_machine stateMachine, string animString) : base(player, stateMachine, animString)
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
        player.Pistol.RotationSway(player.mouseDelta);
    }
}
