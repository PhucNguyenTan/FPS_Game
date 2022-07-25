using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable_obj/Velocity")]
public class Velocity_script : ScriptableObject
{
    public float Up_vcurrent { get; private set; } = 0f;
    public float Forward_vcurrent { get; private set; } = 0f;
    public Vector3 CurrentDirection { get; private set; } = Vector3.zero;

    public bool _isGravitySuspend = true;

    public void TurnOffGravity()
    {
        _isGravitySuspend = true;
    }

    public void TurnOnGravity()
    {
        _isGravitySuspend = false;
    }
    
}
