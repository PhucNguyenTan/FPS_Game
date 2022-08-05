

public class Enemy_state_machine
{
    public Enemy_base_state currentState;

    public void Initialize(Enemy_base_state startingState) 
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(Enemy_base_state newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
