using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun_Rocket : Gun_base
{
    [SerializeField] AudioClip _shootAudio;
    public static UnityAction Shooting;
    [SerializeField] ParticleSystem _muzzle;
    [SerializeField] Explosive_data _rocketData;
    

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
        Vector3 dir = _camTransform.forward;
        bool isImpacted = Physics.Raycast(_camTransform.position, dir, out hit);

        Vector3 direction = isImpacted ? hit.point - transform.position : dir;

        Explosive rocket = Instantiate(_rocketData.ExplosivePrefab, _muzzle.transform.position, _muzzle.transform.rotation);
        rocket.AddDirection(direction.normalized);
        rocket.SetData(_rocketData);
        rocket.Release();



    }
}
