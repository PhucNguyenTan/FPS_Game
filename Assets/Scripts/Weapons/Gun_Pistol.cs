using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Pistol : Gun_base
{
    public Gun_Pistol()
    {
        fireRate = 2f;
    }
    
    

    //private Animator animtor;
    private AudioSource audio;

    public ParticleSystem MuzzleFlash;
    public ParticleSystem impacttest;

    public Recoil recoil_S;
    private string curAnim = "pistol_idle";

    

    public void Awake()
    {
        //animtor = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }


    public void PistolShoot(RaycastHit hit)
    {
        //RecoilFire();
        Shot();
        MuzzleFlash.Play();
        audio.Play();
        recoil_S.RecoilFire();
        if (hit.transform != null)
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(10f);
            }

            ParticleSystem impact = Instantiate(impacttest, hit.point, Quaternion.LookRotation(hit.normal));
            impact.Play();
            Destroy(impact.gameObject, 1f);
        }
        ToShootAgain();
    }

    public void PlayAnim(string newAnim)
    {
        //if (newAnim == curAnim) return;
        //animtor.Play(newAnim);
        //1curAnim = newAnim;
    }

    
}
