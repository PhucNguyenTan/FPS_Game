

public class Player_state_shoot : Player_base_state
{

    public Player_state_shoot(Player player, Player_state_machine stateMachine, Player_data playerData, string animString) : base(player, stateMachine, playerData, animString)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.Pistol.Wait();
        player.UnsubscribeToShoot();
    }

    public override void Exit()
    {
        base.Exit();
        player.Pistol.ResetRotation();
    }

    public override void Logic()
    {
        base.Logic();
        player.Pistol.RecoilUpdate();
        if (player.Pistol.CanShoot)
        {
            player.shootingMachine.ChangeStage(player.stateGunidle);
        }
    }
}
