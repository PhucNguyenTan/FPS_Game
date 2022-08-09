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
    [SerializeField] Explosive_charge_data _energyBallData;
    Explosive_charge _currentProjectile;
    Transform _projectileOrigin;

    bool _isCharging = false;
    float _chargeTimer = 0f;
    float _totalChargeTime = 0f;
    float _maxChargeTime = 3f;


    public Gun_Energy()
    {
        
    }

    private void Update()
    {
        if (_currentProjectile != null)
        {
           _currentProjectile.SetPosition(_projectileOrigin.position);
        }
    }

    private void OnEnable()
    {
        InputHandler.Instance.HybridCharge += StartCharging;
        InputHandler.Instance.HybridShoot += ShootEnergy;
        InputHandler.Instance.HybridChargedShoot += ShootChargedEnergy;
        InputHandler.Instance.HybridCancel += CancelCharge;
        _projectileOrigin = transform.GetChild(0).GetChild(1).transform;
        ResetUnequip();
        Equip();
    }

    private void OnDisable()
    {
        InputHandler.Instance.HybridCharge -= StartCharging;
        InputHandler.Instance.HybridShoot -= ShootEnergy;
        InputHandler.Instance.HybridChargedShoot -= ShootChargedEnergy;
        InputHandler.Instance.HybridCancel -= CancelCharge;
    }

    void ShootChargedEnergy()
    {
        if (!CheckCanShoot() || _isEquiping || _isUnequiping) return;
        AlternateShooting?.Invoke();
        _muzzleCharging.Stop();
        _muzzleCharge.Play();
        SoundManager.Instance.PlayEffectOnce(_chargeShotAudio);

        RaycastHit hit;
        Vector3 dir = _camTransform.forward;
        bool isImpacted = Physics.Raycast(_camTransform.position, dir, out hit);
        Vector3 direction = isImpacted ? hit.point - transform.position : dir;

        _currentProjectile.SetRotation(_muzzle.transform.rotation);
        _currentProjectile.AddDirection(direction.normalized);
        _currentProjectile.Release();
        _currentProjectile = null;

        _isCharging = false;
        _chargeTimer = 0f;
        _totalChargeTime = 0f;


    }

    void ShootEnergy()
    {
        if (!CheckCanShoot()) return;
        Shooting?.Invoke();
        _muzzle.Play();
        SoundManager.Instance.PlayEffectOnce(_shootAudio);
        RaycastHit hit;
        Vector3 dir = _camTransform.forward;
        bool isImpacted = Physics.Raycast(_camTransform.position, dir, out hit);

        Vector3 direction = isImpacted ? hit.point - transform.position : dir ;

        Explosive_charge energyBall = Instantiate(_energyBallData.ExplosivePrefab, _projectileOrigin.position, _muzzle.transform.rotation);
        energyBall.AddDirection(direction.normalized);
        energyBall.SetData(_energyBallData);
        energyBall.Release();
    }

    public void CancelCharge()
    {
        _muzzleCharging.Stop();
        Destroy(_currentProjectile.gameObject);
    }

    public void StartCharging()
    {
        if (!CheckCanShoot()) return;
        _muzzleCharging.Play();

        _currentProjectile = Instantiate(_energyBallData.ExplosivePrefab, _projectileOrigin.position, _projectileOrigin.rotation);
        _currentProjectile.SetData(_energyBallData);
        _isCharging = true;
        _chargeTimer = 0f;
        
    }
}
