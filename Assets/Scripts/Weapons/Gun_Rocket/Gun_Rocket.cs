using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun_Rocket : Gun_base
{

    public static UnityAction Shooting;
    [SerializeField] ParticleSystem _muzzle;
    

    public Gun_Rocket()
    {
        
    }

    private void OnEnable()
    {
        InputHandler.Instance.SingleShoot += Shoot;
    }

    private void OnDisable()
    {
        InputHandler.Instance.SingleShoot -= Shoot;

    }
    void Shoot()
    {
        if(!CheckCanShoot()) return;
        Shooting?.Invoke();
        _muzzle.Play();
    }
}
