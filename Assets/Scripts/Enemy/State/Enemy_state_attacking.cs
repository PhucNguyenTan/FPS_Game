

public class Enemy_state_attacking : Enemy_base_state
{
    public Enemy_state_attacking(Enemy enemy, Enemy_state_machine stateMachine, string stringAnim) : base(enemy, stateMachine, stringAnim)
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
        if(enemy.timeSinceLastShot > enemy.coolDownTime)
        {
            enemy.CreateProjectile();
            enemy.ResetCountDown();
        }
        else
        {
            enemy.CountingDown();
        }


    }
}
