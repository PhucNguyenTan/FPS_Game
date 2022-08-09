using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable_obj/Explosion")]
public class Explosion_data : ScriptableObject
{
    [Header("Explosive")]
    public float BlastRadius;
    public float Duration;
    public Color BlastColor;
    public AnimationCurve AlphaFadingCurve;
    public Explosion_base Explosion;
    public LayerMask PushableLayers;
}
