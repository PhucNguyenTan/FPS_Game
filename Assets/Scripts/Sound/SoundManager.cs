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

    private void Start()
    {
        _effectSource.volume = 0.3f;
    }

    public void PlayEffectOnce(AudioClip clip)
    {
        _effectSource.PlayOneShot(clip);
    }
}
