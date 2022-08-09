using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive_charge : Projectile_base
{
    Explosive_data _projectile;
    Explosion_data _explosion;

    int _maxLevel;
    float _maxChargeTime;
    float _additionalRadius;
    float _additionalSize;

    bool _isLevelupAble = true;
    int _level = 0;
    float _chargeTimer = 0f;

    protected override void Update()
    {
        base.Update();
        if(_isLevelupAble)
        {
            if (_chargeTimer >= _maxChargeTime)
            {
                UpLevel();
                _chargeTimer = 0f;
            }
            _chargeTimer += Time.deltaTime;
        }
        transform.localScale = Vector3.one * _scale;
    }

    protected override void LogicOnColliding()
    {
        Instantiate(_explosion.Explosion, transform.position, Quaternion.identity).SetExplosionData(_explosion).AddRadius(_additionalRadius);
    }

    public void SetData(Explosive_charge_data data)
    {
        SetGeneralData(data);
        _explosion = data.ExplosionData;
        _maxLevel = data.MaxLevel;
        _maxChargeTime = data.MaxChargeTimePerLevel;
        _additionalRadius = data.RadiusIncremetalPerLevel;
        _additionalSize = data.ScaleIncremetalPerLevel;
    }


    void UpLevel()
    {
        if (_level == _maxLevel) return;
        _level++;
        _scale += _additionalSize;
        _additionalRadius += 1f;
    }

    public void ReleaseCharge()
    {
        Release();
        _isLevelupAble = false;
    }
}
