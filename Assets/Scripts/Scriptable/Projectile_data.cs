using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable_obj/ProjectileData")]
public class Projectile_data : ScriptableObject
{
    public float Damage;
    public float Speed;
    public GameObject Shape;
    public LayerMask LayerMasks;
    public Explosion_data ExplosionData;
    public float MaxLifeSpan;
    public float Scale;
}
