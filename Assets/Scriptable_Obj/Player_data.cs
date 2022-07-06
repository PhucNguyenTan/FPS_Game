using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable_obj/Player_data")]
public class Player_data : ScriptableObject
{
    [Header("Player")]
    public float MaxHealth = 50f;
    public float MaxJumpHeight = 1f;
    public float MaxJumpTime = 2f;
    public float CrouchHeight;
    public float StandHeight;

    [Header("Control")]
    public float MouseSensitivity = 0.3f;
    public float MoveSpeed = 5f;

    //Might be put in different section
    public float GroundGravity = -0.5f;
    public float EarthGravity = -9.8f;
    

    
    

    

    
}
