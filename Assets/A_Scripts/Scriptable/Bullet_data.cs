using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable_obj/Bullet")]
public class Bullet_data : Projectile_data
{
    public ParticleSystem ShotImpact;
    public Bullet BulletObj;
}
