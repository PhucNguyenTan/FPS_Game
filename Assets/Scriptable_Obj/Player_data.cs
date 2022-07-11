using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable_obj/Player_data")]
public class Player_data : ScriptableObject
{
    [Header("Player")]
    public float JumpHeight = .01f;
    public float JumpTime = 1f;
    public float DropHeight = 1f;
    public float DropTime = 2f;
    public float DashSpeed = 3f;
    public float DashFriction = 1f;
    public float SlideFriction = .2f;

    [Header("Control")]
    public float MouseSensitivity = .3f;
    public float MoveSpeed = 5f;
    public float CrouchMoveSpeed = 2f;
    public float JoystickCamSpeed = 5f;

    [Header("Crouch")]
    public float CrouchSpeed = .3f;
    public float CrouchHeight = 1f;
    public float StandHeight = 2f;
    public float camCrouchHeight = .5f;
    public float camStandHeight = 1f;


    //Might be put in different object
    public float GroundGravity = -0.5f;
    public float EarthGravity = -9.8f;
    
}
