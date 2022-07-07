using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable_obj/Player_data")]
public class Player_data : ScriptableObject
{
    [Header("Player")]
    public float JumpHeight = 0.01f;
    public float JumpTime = 1f;
    public float CrouchHeight;
    public float StandHeight;
    public float DropHeight = 1f;
    public float DropTime = 2f;
    public float DashSpeed = 3f;
    public float Friction = 1f;

    [Header("Control")]
    public float MouseSensitivity = 0.3f;
    public float MoveSpeed = 5f;

    //Might be put in different object
    public float GroundGravity = -0.5f;
    public float EarthGravity = -9.8f;
    
}
