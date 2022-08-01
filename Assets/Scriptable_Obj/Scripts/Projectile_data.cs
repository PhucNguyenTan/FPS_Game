using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable_obj/ProjectileData")]
public class Projectile_data : ScriptableObject
{
    public float Damage;
    public float BlastRadius;
    public float Speed;
    public GameObject Shape;
}
