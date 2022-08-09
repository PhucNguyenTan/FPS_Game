using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile_data : ScriptableObject
{
    [Header("General")]
    public float Damage;
    public float Speed;
    public LayerMask Touchable;
    public LayerMask Damagable;
    public float MaxLifeSpan;
    public float Scale;
}
