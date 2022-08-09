using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Projectile_base
{ 
    Explosion_data _explosion;

    protected override void LogicOnColliding()
    {
        Instantiate(_explosion.Explosion, transform.position, Quaternion.identity).SetExplosionData(_explosion);
    }

    public void SetData(Explosive_data data)
    {
        SetGeneralData(data);
        _explosion = data.ExplosionData;
    }
}
