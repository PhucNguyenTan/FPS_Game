using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static GameState State = GameState.Run;

    public static event UnityAction<GameState> OnChangeState;

    public enum GameState{
        CountDown,
        Run,
        Pause,
        Fail,
        Win,
        End
    }

    public void ChangeGameState(GameState newState)
    {
        State = newState;
        switch (State)
        {
            case GameState.CountDown:
                break;
        }
    }


}
