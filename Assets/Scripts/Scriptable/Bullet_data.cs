using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable_obj/Bullet")]
public class Bullet_data : MonoBehaviour
{
    public ParticleSystem ImpactEffect;
    public bool IsExplosive { get; private set; } = false;
}
