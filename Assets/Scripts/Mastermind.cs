using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mastermind : MonoBehaviour
{
    public e_Difficulty difficulty;
    public e_GameMode gameMode;

    


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
