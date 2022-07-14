using UnityEngine;

public abstract class Player_base_state
{
    protected Player player;
    protected Player_state_machine stateMachine;
    protected string animString;
    protected float startTime;
    protected Player_data playerData;
    public Player_base_state(Player player, Player_state_machine stateMachine, Player_data playerData, string animString)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animString = animString;
        this.playerData = playerData;
    }

    public virtual void Enter()
    {
        startTime = Time.time;
        if (playerData.ShowStateName)
        {
            Debug.Log(animString);
        }
        
    }

    public virtual void Exit()
    {

    }

    public virtual void Logic()
    {

    }
}
