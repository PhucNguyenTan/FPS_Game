using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundOnStart : MonoBehaviour
{
    [SerializeField] AudioClip _clip;

    private void Start()
    {
        SoundManager.Instance.PlayEffectOnce(_clip);
    }
}
