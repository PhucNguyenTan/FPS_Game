

public class Player_state_shoot : Player_base_state
{

    public Player_state_shoot(Player player, Player_state_machine stateMachine, Player_data playerData, string animString) : base(player, stateMachine, playerData, animString)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.CurrentGun.Wait();
        player.UnsubscribeToShoot();
    }

    public override void Exit()
    {
        base.Exit();
        player.CurrentGun.ResetRotation();
    }

    public override void Logic()
    {
        base.Logic();
        player.CurrentGun.RecoilUpdate();
        if (player.CurrentGun.CanShoot)
        {
            player.shootingMachine.ChangeStage(player.stateGunidle);
        }
    }
}
