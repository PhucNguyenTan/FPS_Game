using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun_Shotgun : Gun_base
{
    public static UnityAction Shooting;
    [SerializeField] int _numberOfShot = 10;
    [SerializeField] float _maxSpread = 0.05f;
    [SerializeField] AudioClip _shootAudio;
    [SerializeField] ParticleSystem _muzzleFlash;
    [SerializeField] Bullet_data _bulletData;

    public Gun_Shotgun()
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
        _muzzleFlash.Play();
        SoundManager.Instance.PlayEffectOnce(_shootAudio);
        RaycastHit[] hits = new RaycastHit[_numberOfShot];
        for(int i = 0; i<_numberOfShot; i++)
        {
            Vector3 dir = _camTransform.forward +
                new Vector3(Random.Range(-_maxSpread, _maxSpread),
                            Random.Range(-_maxSpread, _maxSpread),
                            Random.Range(-_maxSpread, _maxSpread));

            bool isImpacted = Physics.Raycast(_camTransform.position, dir, out hits[i]);
            Vector3 direction = isImpacted ?  hits[i].point - transform.position : dir;

            Bullet bullet = Instantiate(_bulletData.BulletObj, _muzzleFlash.transform.position, _muzzleFlash.transform.rotation);
            bullet.AddDirection(direction.normalized);
            bullet.SetData(_bulletData).Release();
        }
    }
}
