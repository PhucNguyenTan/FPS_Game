using UnityEngine;

public abstract class Player_base_state
{
    protected Player player;
    protected Player_state_machine stateMachine;
    protected string animString;
    protected float startTime;
    public Player_base_state(Player player, Player_state_machine stateMachine, string animString)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animString = animString;
    }

    public virtual void Enter()
    {
        startTime = Time.time;
        Debug.Log(animString);
    }

    public virtual void Exit()
    {

    }

    public virtual void Logic()
    {

    }
}
