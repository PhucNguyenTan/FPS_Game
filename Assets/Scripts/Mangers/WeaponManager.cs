using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Gun_base CurrentWeapon { get; private set; }
    [SerializeField] Gun_Smg _smg;
    [SerializeField] Gun_Shotgun _shotgun;
    [SerializeField] Gun_Rocket _rocket;
    [SerializeField] Gun_Energy _energy;
    List<Gun_base> _listGun;
    int _lastGunNum = 0;
    int _currentGunNum = 2;

    private void Awake()
    {
        _listGun = new List<Gun_base>();
        _listGun.Add(_energy);
        _listGun.Add(_shotgun);
        _listGun.Add(_smg);
        _listGun.Add(_rocket);
    }

    private void Start()
    {
        CurrentWeapon = _listGun[_currentGunNum];
        CurrentWeapon.Equip();
    }

    private void OnEnable()
    {
        InputHandler.Instance.NextWeapon += GetNextWeapon;
        InputHandler.Instance.PrevWeapon += GetPrevUsedWeapon;
    }

    private void OnDisable()
    {
        InputHandler.Instance.NextWeapon -= GetNextWeapon;
        InputHandler.Instance.PrevWeapon -= GetPrevUsedWeapon;

    }

    public void GetNextWeapon()
    {
        _lastGunNum = _currentGunNum;
        _currentGunNum = _currentGunNum == _listGun.Count? 0 : _currentGunNum++;
        CurrentWeapon.Unequip();
        CurrentWeapon = _listGun[_currentGunNum];
        CurrentWeapon.Equip();
    }

    public void GetPrevWeapon()
    {
        _lastGunNum = _currentGunNum;
        _currentGunNum = _currentGunNum == 0 ? _listGun.Count : _currentGunNum--;
        CurrentWeapon.Unequip();
        CurrentWeapon = _listGun[_currentGunNum];
        CurrentWeapon.Equip();
    }

    public void GetWeaponNumber(int number)
    {
        if (_currentGunNum == number)
            return;
        if (_currentGunNum < 0 || _currentGunNum > _listGun.Count)
            //throw error
            Debug.Log("error");

        _lastGunNum = _currentGunNum;
        _currentGunNum = _currentGunNum == 0 ? _listGun.Count : _currentGunNum--;
        CurrentWeapon.Unequip();
        CurrentWeapon = _listGun[_currentGunNum];
        CurrentWeapon.Equip();
    }

    public void GetPrevUsedWeapon()
    {
        int temp = _currentGunNum;
        _currentGunNum = _lastGunNum;
        _lastGunNum = temp;
        CurrentWeapon.Unequip();
        CurrentWeapon = _listGun[_currentGunNum];
        CurrentWeapon.Equip();
    }
}
