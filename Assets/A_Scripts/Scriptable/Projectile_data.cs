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
    public LayerMask Touchable;
    public LayerMask Damagable;
    public float MaxLifeSpan;
    public float Scale;

    [Header("Style")]
    public bool IsExplosive;
    public bool IsLevelupAble;
    public Explosion_data ExplosionData;
    public Explosion_base Explosion;
    public ParticleSystem ImpactEffect;
    
}
