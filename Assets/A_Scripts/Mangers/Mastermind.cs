using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mastermind : MonoBehaviour
{
    [SerializeField] e_Difficulty difficulty;
    [SerializeField] e_GameMode gameMode;

    


    #region enum
    public enum e_Difficulty
    {
        easy,
        normal,
        hard
    }

    public enum e_GameMode { 
        horde,
        arcade
    }
    #endregion

}
