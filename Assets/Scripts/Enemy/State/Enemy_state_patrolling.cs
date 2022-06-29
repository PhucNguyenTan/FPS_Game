

public class Enemy_state_patrolling : Enemy_base_state
{
    public Enemy_state_patrolling(Enemy enemy, Enemy_state_machine state_Machine, string stringAnim) : base(enemy, state_Machine, stringAnim)
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
        enemy.ChasePlayer();
    }
}
