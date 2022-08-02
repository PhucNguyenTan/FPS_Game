using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable_obj/Explosion")]
public class Explosion_data : ScriptableObject
{
    //material
    //BlastRadius
    //Damage
    //Time lapse
    public float BlastRadius;
    public float Duration;
    public Color BlastColor;
    public AnimationCurve AlphaFadingCurve;
}
