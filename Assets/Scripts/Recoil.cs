using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{

    Vector3 _currentRotation;
    Vector3 TargetRotation;
    [SerializeField] Weapon_script _smgData;
    [SerializeField] Weapon_script _shotgunData;
    [SerializeField] Weapon_script _energyData;
    [SerializeField] Weapon_script _rocketData;
    Weapon_script _currentWeapon;


    private void Update()
    {
        if (TargetRotation != Vector3.zero)
        {
            TargetRotation = Vector3.Lerp(TargetRotation, Vector3.zero, _currentWeapon.ReturnSpeed * Time.deltaTime);
            _currentRotation = Vector3.Slerp(_currentRotation, TargetRotation, _currentWeapon.ReturnSpeed * Time.fixedDeltaTime);// ???
            transform.localRotation = Quaternion.Euler(_currentRotation);
        }
    }


    void SetRecoilInitial()
    {
        TargetRotation = new Vector3(_currentWeapon.RecoilX,
                    Random.Range(-_currentWeapon.RecoilY, _currentWeapon.RecoilY), 
                    Random.Range(-_currentWeapon.RecoilZ, _currentWeapon.RecoilZ));
    }

    void SetAlternativeRecoilInitial()
    {
        TargetRotation = new Vector3(_currentWeapon.A_RecoilX,
                    Random.Range(-_currentWeapon.A_RecoilY, _currentWeapon.A_RecoilY),
                    Random.Range(-_currentWeapon.A_RecoilZ, _currentWeapon.A_RecoilZ));
    }

    void ShotgunRecoil()
    {
        _currentWeapon = _shotgunData;
        SetRecoilInitial();
    }

    void SmgRecoil()
    {
        _currentWeapon = _smgData;
        SetRecoilInitial();
    }

    void RocketRecoil()
    {
        _currentWeapon = _rocketData;
        SetRecoilInitial();
    }

    void ChargeRecoil()
    {
        _currentWeapon = _energyData;
        SetAlternativeRecoilInitial();
    }

    void EnergyRecoil()
    {
        _currentWeapon = _energyData;
        SetRecoilInitial();
    }


    private void OnEnable()
    {
        Gun_Shotgun.Shooting += ShotgunRecoil;
        Gun_Rocket.Shooting += RocketRecoil;
        Gun_Smg.Shooting += SmgRecoil;
        Gun_Energy.Shooting += EnergyRecoil;
        Gun_Energy.AlternateShooting += ChargeRecoil;
    }

    private void OnDisable()
    {
        Gun_Shotgun.Shooting -= ShotgunRecoil;
        Gun_Rocket.Shooting -= RocketRecoil;
        Gun_Smg.Shooting -= SmgRecoil;
        Gun_Energy.Shooting -= EnergyRecoil;
        Gun_Energy.AlternateShooting -= ChargeRecoil;
    }
}
