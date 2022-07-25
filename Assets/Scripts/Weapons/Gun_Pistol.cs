using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun_Pistol : Gun_base
{

    public static UnityAction Shooting;
    [SerializeField] AudioClip _shootAudio;
    [SerializeField] ParticleSystem _muzzleFlash;
    [SerializeField] ParticleSystem _impactEffect;

    public Gun_Pistol()
    {
        fireRate = 250;
    }
      

    public override void Shoot(Transform originPoint)
    {
        RaycastHit hit;
        Physics.Raycast(originPoint.position, originPoint.forward, out hit);
        Shooting?.Invoke();
        Shot();
        _muzzleFlash.Play();
        SoundManager.Instance.PlayEffectOnce(_shootAudio);
        if (hit.transform != null)
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(10f);
            }

            ParticleSystem impact = Instantiate(_impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            impact.Play();
            Destroy(impact.gameObject, 1f);
        }
    }
}
