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
    [SerializeField] ParticleSystem _impactFlash;

    public Gun_Shotgun()
    {
        _fireRate = 500;
    }

    private void OnEnable()
    {
        InputHandler.Instance.SingleShoot += Shoot;
    }

    private void OnDisable()
    {
        InputHandler.Instance.SingleShoot -= Shoot;

    }

    public void Shoot()
    {
        Shooting?.Invoke();
        Shot();
        _muzzleFlash.Play();
        SoundManager.Instance.PlayEffectOnce(_shootAudio);
        RaycastHit[] hits = new RaycastHit[_numberOfShot];
        for(int i = 0; i<_numberOfShot; i++)
        {
            Vector3 dir = _camTransform.forward +
                new Vector3(Random.Range(-_maxSpread, _maxSpread),
                            Random.Range(-_maxSpread, _maxSpread),
                            Random.Range(-_maxSpread, _maxSpread));
            Physics.Raycast(_camTransform.position, dir, out hits[i]);
            if (hits[i].transform != null)
            {
                Enemy enemy = hits[i].transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(2f);
                }

                ParticleSystem impact = Instantiate(_impactFlash, hits[i].point, Quaternion.LookRotation(hits[i].normal));
                impact.Play();
                Destroy(impact.gameObject, 0.35f);
            }
        }
    }
}
