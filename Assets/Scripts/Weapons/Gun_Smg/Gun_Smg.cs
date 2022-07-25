using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun_Smg : Gun_base
{
    [SerializeField] float _maxSpread = 0.01f;
    [SerializeField] AudioClip _shootAudio;
    [SerializeField] ParticleSystem _muzzleFlash;
    [SerializeField] ParticleSystem _impactFlash;
    [SerializeField] float damageDeal = 2f;

    public static UnityAction Shooting;
    public Gun_Smg()
    {
        fireRate = 500;
    }

    public override void Shoot(Transform originPoint)
    {
        Shooting?.Invoke();
        Shot();
        _muzzleFlash.Play();
        SoundManager.Instance.PlayEffectOnce(_shootAudio);

        RaycastHit hit;
        Vector3 dir = originPoint.forward +
               new Vector3(Random.Range(-_maxSpread, _maxSpread),
                           Random.Range(-_maxSpread, _maxSpread),
                           Random.Range(-_maxSpread, _maxSpread));
        bool isImpacted = Physics.Raycast(originPoint.position, dir, out hit);
        if (isImpacted)
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageDeal);
            }
        }
        ParticleSystem impact = Instantiate(_impactFlash, hit.point, Quaternion.LookRotation(hit.normal));
        impact.Play();
        Destroy(impact.gameObject, 0.2f);
    }
}
