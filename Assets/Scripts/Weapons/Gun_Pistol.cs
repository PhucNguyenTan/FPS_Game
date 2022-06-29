using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Pistol : MonoBehaviour
{
    public float damage = 10f;
    public float range = 50f;
    public int fireRate = 5;
    

    public Animation Firing;

    private Animator animtor;
    private AudioSource audio;

    public ParticleSystem MuzzleFlash;
    public ParticleSystem impacttest;

    public Recoil recoil_S;
    private string curAnim = "pistol_idle";

    public void Awake()
    {
        animtor = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }


    public void PistolShoot(RaycastHit hit)
    {
        PlayAnim("Fire_Anim");
        MuzzleFlash.Play();
        audio.Play();
        recoil_S.recoilFire();
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
    }

    public void PlayAnim(string newAnim)
    {
        if (newAnim == curAnim) return;
        animtor.Play(newAnim);
        curAnim = newAnim;
    }

    
}
