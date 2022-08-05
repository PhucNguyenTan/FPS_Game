using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Scriptable_obj/Velocity")]
public class Velocity_script : ScriptableObject
{
    public float Vertical_vcurrent  = 0f;
    public float Horizontal_vcurrent  = 0f;
    public Vector3 CurrentDirection  = Vector3.zero;
    public bool _isGravitySuspend = true;

    public UnityEvent<float> OnVerticalVelChange;
    public UnityEvent<float> OnHorizontalVelChange;
    public UnityEvent<Vector3> OnDirectionChange;





    private void OnEnable()
    {
        if (OnVerticalVelChange == null)
            OnVerticalVelChange = new UnityEvent<float>();
        if (OnHorizontalVelChange == null)
            OnHorizontalVelChange = new UnityEvent<float>();
        if (OnDirectionChange == null)
            OnDirectionChange = new UnityEvent<Vector3>();
    }

    public void UpdateVertical(float newVvelocity)
    {
        Vertical_vcurrent = newVvelocity;
        OnVerticalVelChange?.Invoke(Vertical_vcurrent);
    }

    public void AddClampVertical(float v_add, float clamp)
    {
        Vertical_vcurrent += v_add;
        Vertical_vcurrent = Mathf.Max(Vertical_vcurrent, clamp);
        OnVerticalVelChange?.Invoke(Vertical_vcurrent);
    }

    public void UpdateHorizontal(float newHvelocity)
    {
        Horizontal_vcurrent = newHvelocity;
        OnHorizontalVelChange?.Invoke(Horizontal_vcurrent);
    }

    public void UpdateDirection(Vector3 newDirection)
    {
        CurrentDirection = newDirection;
        OnDirectionChange?.Invoke(CurrentDirection);
    }

    public void TurnOffGravity()
    {
        _isGravitySuspend = true;
    }

    public void TurnOnGravity()
    {
        _isGravitySuspend = false;
    }
    
}
