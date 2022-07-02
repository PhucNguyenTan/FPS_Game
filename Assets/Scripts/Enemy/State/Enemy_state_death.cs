

public class Enemy_state_death : Enemy_base_state
{
    public Enemy_state_death(Enemy enemy, Enemy_state_machine  stateMachine, string animString) : base(enemy, stateMachine, animString)
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
        enemy.DestroyEnemy(2f);
    }
}
