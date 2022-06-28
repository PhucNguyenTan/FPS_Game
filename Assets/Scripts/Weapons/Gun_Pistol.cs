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

    public void Awake()
    {
        animtor = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }


    public void PistolShoot(RaycastHit hit)
    {
        animtor.Play("Fire_Anim");
        MuzzleFlash.Play();
        audio.Play();
        if (hit.transform != null)
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(10f);
            }

            ParticleSystem impact = Instantiate(impacttest, hit.point, Quaternion.LookRotation(hit.normal));
            impact.Play();
            Destroy(impact, 2f);
        }
    }
}
