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
    [SerializeField] ParticleSystem _impactFlash;


    public Gun_Smg()
    {
        
    }

    private void OnEnable()
    {
        InputHandler.Instance.AutoShoot += Shoot;
    }

    private void OnDisable()
    {
        InputHandler.Instance.AutoShoot -= Shoot;

    }

    public  void Shoot()
    {
        if (!CheckCanShoot()) return;
        Shooting?.Invoke();
        Shot();
        _muzzleFlash.Play();
        SoundManager.Instance.PlayEffectOnce(_shootAudio);

        RaycastHit hit;
        Vector3 dir = _camTransform.forward +
               new Vector3(Random.Range(-_maxSpread, _maxSpread),
                           Random.Range(-_maxSpread, _maxSpread),
                           Random.Range(-_maxSpread, _maxSpread));
        bool isImpacted = Physics.Raycast(_camTransform.position, dir, out hit);
        if (isImpacted)
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(_weaponData.Damage);
            }
        }
        ParticleSystem impact = Instantiate(_impactFlash, hit.point, Quaternion.LookRotation(hit.normal));
        impact.Play();
        Destroy(impact.gameObject, 0.2f);
    }
}
