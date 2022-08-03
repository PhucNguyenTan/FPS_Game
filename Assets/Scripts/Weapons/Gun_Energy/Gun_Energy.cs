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
    [SerializeField] Projectile_data _projectileData;
    [SerializeField] Projectile_base _projectile;
    Projectile_base _currentProjectile;
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
            _currentProjectile = _currentProjectile.SetPosition(_projectileOrigin.position);
            _chargeTimer += Time.deltaTime;
            if(_chargeTimer >= 0.5f && _totalChargeTime < _maxChargeTime)
            {
                _currentProjectile.AddScale(0.01f);
                _totalChargeTime += _chargeTimer;
                _chargeTimer = 0f;
            }
        }
    }

    private void OnEnable()
    {
        InputHandler.Instance.HybridCharge += Charging;
        InputHandler.Instance.HybridShoot += ShootSmall;
        InputHandler.Instance.HybridChargedShoot += ShootEnergy;
        InputHandler.Instance.HybridCancel += CancelCharge;
        _projectileOrigin = transform.GetChild(0).GetChild(1).transform;
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

        RaycastHit hit;
        Vector3 castDir = _camTransform.forward;
        bool isImpacted = Physics.Raycast(_camTransform.position, castDir, out hit);

        Vector3 direction = isImpacted ? hit.point - transform.position : castDir * 1000f - transform.position;


        _currentProjectile.AddDirection(direction.normalized).SetRotation(_muzzle.transform.rotation).Release();
        _currentProjectile = null;
        _isCharging = false;
        _chargeTimer = 0f;
        _totalChargeTime = 0f;


    }

    void ShootSmall()
    {
        if (!CheckCanShoot()) return;
        Shooting?.Invoke();
        _muzzle.Play();
        SoundManager.Instance.PlayEffectOnce(_shootAudio);
        RaycastHit hit;
        Vector3 castDir = _camTransform.forward;
        bool isImpacted = Physics.Raycast(_camTransform.position, castDir, out hit);

        Vector3 direction = isImpacted ? hit.point - transform.position : castDir * 100f - transform.position;

        Projectile_base bullet = Instantiate(_projectile, _projectileOrigin.position, _muzzle.transform.rotation)
            .AddDirection(direction.normalized).SetProjectileData(_projectileData).Release();
    }

    public void CancelCharge()
    {
        _muzzleCharging.Stop();
        Destroy(_currentProjectile.gameObject);
    }

    public void Charging()
    {
        if (!CheckCanShoot()) return;
        _muzzleCharging.Play();
        
        _currentProjectile = Instantiate(_projectile, _projectileOrigin.position, _projectileOrigin.rotation)
        .SetProjectileData(_projectileData);
        _isCharging = true;
        _chargeTimer = 0f;
        
    }
}
