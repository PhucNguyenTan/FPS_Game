using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun_Rocket : Gun_base
{
    [SerializeField] AudioClip _shootAudio;
    public static UnityAction Shooting;
    [SerializeField] ParticleSystem _muzzle;
    [SerializeField] Projectile_data _projectilteData;
    [SerializeField] Projectile_base _currentBullet;
    

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
        RaycastHit hit;
        Vector3 castDir = _camTransform.forward;
        bool isImpacted = Physics.Raycast(_camTransform.position, castDir, out hit);

        Vector3 direction = isImpacted ? hit.point - transform.position : castDir * 100f - transform.position;

        Projectile_base bullet = Instantiate(_currentBullet, transform.position, transform.rotation)
            .AddDirection(direction.normalized).SetProjectileData(_projectilteData).Release();

        

    }
}
