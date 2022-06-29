

public class Enemy_base_state
{
    protected Enemy enemy;
    protected Enemy_state_machine stateMachine;
    protected string stringAnim;

    public Enemy_base_state(Enemy enemy, Enemy_state_machine stateMachine, string stringAnim)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
        this.stringAnim = stringAnim;
    }

    public virtual void Enter()
    {

    }
    
    public virtual void Exit()
    {

    }

    public virtual void Logic()
    {

    }

}
