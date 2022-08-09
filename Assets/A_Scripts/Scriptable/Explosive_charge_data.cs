using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable_obj/EnergyBall")]
public class Explosive_charge_data : Projectile_data
{
    public Explosion_data ExplosionData;
    public Explosive_charge ExplosivePrefab;

    [Header("Level Details")]
    public int MaxLevel;
    public float MaxChargeTimePerLevel;
    public float RadiusIncremetalPerLevel;
    public float ScaleIncremetalPerLevel;
}
