using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //Singleton
    public static SoundManager Instance;

    [SerializeField] AudioSource _effectSource;
    [SerializeField] AudioSource _musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void PlayEffectOnce(AudioClip clip, float volume)
    {
        _effectSource.volume = volume;
        _effectSource.PlayOneShot(clip);
    }
}
