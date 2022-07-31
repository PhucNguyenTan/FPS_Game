using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun_Rocket : Gun_base
{
    [SerializeField] AudioClip _shootAudio;
    public static UnityAction Shooting;
    [SerializeField] ParticleSystem _muzzle;
    

    public Gun_Rocket()
    {
        
    }

    private void OnEnable()
    {
        InputHandler.Instance.SingleShoot += Shoot;
        ResetUnequip();
        Equip();
    }

    private void OnDisable()
    {
        InputHandler.Instance.SingleShoot -= Shoot;

    }
    public override void Shoot()
    {
        if (!CheckCanShoot() || _isEquiping || _isUnequiping) return;
        Shooting?.Invoke();
        _muzzle.Play();
        SoundManager.Instance.PlayEffectOnce(_shootAudio);

    }
}
