using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile_base
{
    ParticleSystem _bulletImpact;

    protected override void LogicOnColliding()
    {
        ParticleSystem impact = Instantiate(_bulletImpact, transform.position, Quaternion.LookRotation(_impactNormal));
        impact.Play();
        Destroy(impact.gameObject, 0.2f);
    }

    public Bullet SetData(Bullet_data data)
    {
        SetGeneralData(data);
        _bulletImpact = data.ShotImpact;
        return this;
    }
}
