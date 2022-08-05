using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable_obj/ProjectileData")]
public class Projectile_data : ScriptableObject
{
    [Header("General")]
    public float Damage;
    public float Speed;
    public GameObject Shape;
    public LayerMask LayerMasks;
    public float MaxLifeSpan;
    public float Scale;
    public TrailRenderer Trail;

    [Header("Style")]
    public bool IsExplosive;
    public Explosion_data ExplosionData;
    public Explosion_base Explosion;
    public ParticleSystem ImpactEffect;
    
}
