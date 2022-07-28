using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable_obj/Weapon")]
public class Weapon_script : ScriptableObject
{
    [Header("Recoil")]
    public float RecoilX;
    public float RecoilY;
    public float RecoilZ;
    public float Snapiness;
    public float ReturnSpeed;

    [Header("Recoil_alternative_fireMode")]
    public float A_RecoilX;
    public float A_RecoilY;
    public float A_RecoilZ;
    public float A_Snapiness;
    public float A_ReturnSpeed;

    [Header("Kickback")]
    public float LerpTime;
    public Vector3 TargetPos;
    public Vector3 TargetRos;
    public AnimationCurve KickCurve;

    [Header("Kickback_alt")]
    public float A_LerpTime;
    public Vector3 A_TargetPos;
    public Vector3 A_TargetRos;
    public AnimationCurve A_KickCurve;


    [Header("Property")]
    public float Range;
    public float Damage;
    public float FireRate; //Time between shot in milisecond
    public Vector3 DefaultPosition;
    public Quaternion DefaultRotation;
}
