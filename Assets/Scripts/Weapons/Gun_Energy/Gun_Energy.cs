using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Gun_Energy : Gun_base
{

    public static UnityAction Shooting;
    public static UnityAction AlternateShooting;

    [SerializeField] AudioClip _shootAudio;
    [SerializeField] AudioClip _chargeShotAudio;
    [SerializeField] AudioClip _chargineAudio;
    [SerializeField] AudioClip _cancelChargeAudio;
    [SerializeField] ParticleSystem _muzzle;
    [SerializeField] ParticleSystem _muzzleCharge;
    [SerializeField] ParticleSystem _muzzleCharging;

    public Gun_Energy()
    {
        
    }
    private void OnEnable()
    {
        InputHandler.Instance.HybridCharge += Charging;
        InputHandler.Instance.HybridShoot += ShootSmall;
        InputHandler.Instance.HybridChargedShoot += ShootEnergy;
        InputHandler.Instance.HybridCancel += CancelCharge;
        ResetUnequip();
        Equip();
    }

    private void OnDisable()
    {
        InputHandler.Instance.HybridCharge -= Charging;
        InputHandler.Instance.HybridShoot -= ShootSmall;
        InputHandler.Instance.HybridChargedShoot -= ShootEnergy;
        InputHandler.Instance.HybridCancel -= CancelCharge;
    }

    void ShootEnergy()
    {
        if (!CheckCanShoot() || _isEquiping || _isUnequiping) return;
        AlternateShooting?.Invoke();
        _muzzleCharging.Stop();
        _muzzleCharge.Play();
        SoundManager.Instance.PlayEffectOnce(_chargeShotAudio);
    }

    void ShootSmall()
    {
        if (!CheckCanShoot()) return;
        Shooting?.Invoke();
        _muzzle.Play();
        SoundManager.Instance.PlayEffectOnce(_shootAudio);

    }

    public void CancelCharge()
    {
        _muzzleCharging.Stop();
    }

    public void Charging()
    {
        if (!CheckCanShoot()) return;
        _muzzleCharging.Play();
    }
}
