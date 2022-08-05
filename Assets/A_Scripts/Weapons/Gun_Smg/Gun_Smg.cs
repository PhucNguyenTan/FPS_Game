using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun_Smg : Gun_base
{
    public static UnityAction Shooting;
    [SerializeField] float _maxSpread = 0.01f;
    [SerializeField] AudioClip _shootAudio;
    [SerializeField] ParticleSystem _muzzleFlash;
    [SerializeField] Projectile_base _bullet;
    [SerializeField] Projectile_data _bulletData;


    public Gun_Smg()
    {
        
    }

    private void OnEnable()
    {
        InputHandler.Instance.AutoShoot += Shoot;
        WeaponManager.Unequip += Unequip;

        ResetUnequip();
        Equip();
    }

    private void OnDisable()
    {
        InputHandler.Instance.AutoShoot -= Shoot;
        WeaponManager.Unequip -= Unequip;

    }

    public override void Shoot()
    {
        if (!CheckCanShoot() || _isEquiping || _isUnequiping) return;
        Shooting?.Invoke();
        _muzzleFlash.Play();
        SoundManager.Instance.PlayEffectOnce(_shootAudio);

        RaycastHit hit;
        Vector3 dir = _camTransform.forward +
               new Vector3(Random.Range(-_maxSpread, _maxSpread),
                           Random.Range(-_maxSpread, _maxSpread),
                           Random.Range(-_maxSpread, _maxSpread));

        bool isImpacted = Physics.Raycast(_camTransform.position, dir, out hit);

        Vector3 direction = isImpacted ?  hit.point - transform.position : dir;

        Projectile_base bullet = Instantiate(_bullet, _muzzleFlash.transform.position, _muzzleFlash.transform.rotation)
            .AddDirection(direction.normalized).SetProjectileData(_bulletData).Release();



    }
}
