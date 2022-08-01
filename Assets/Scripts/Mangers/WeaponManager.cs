using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    public Gun_base CurrentWeapon { get; private set; }
    [SerializeField] Gun_Smg _smg;
    [SerializeField] Gun_Shotgun _shotgun;
    [SerializeField] Gun_Rocket _rocket;
    [SerializeField] Gun_Energy _energy;
    public static UnityAction Unequip;
    List<Gun_base> _listGun;
    int _lastGunNum = 0;
    int _currentGunNum = 3;
    bool _isUnequiping;
    bool _isEquiping;

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
        CurrentWeapon.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (CurrentWeapon.IsDoneUnquipping)
        {
            CurrentWeapon.gameObject.SetActive(false);
            CurrentWeapon = _listGun[_currentGunNum];
            CurrentWeapon.gameObject.SetActive(true);
            SubscribeInput();  
        }
    }

    void ToEquip()
    {
        
        CurrentWeapon.Equip();
    }

    private void OnEnable()
    {
        SubscribeInput();
    }

    private void OnDisable()
    {
        UnsubscribeInput();
    }

    public void SubscribeInput()
    {
        InputHandler.Instance.NextWeapon += GetNextWeapon;
        InputHandler.Instance.PrevWeapon += GetPrevWeapon;
    }

    public void UnsubscribeInput()
    {
        InputHandler.Instance.NextWeapon -= GetNextWeapon;
        InputHandler.Instance.PrevWeapon -= GetPrevWeapon;
    }

    public void GetNextWeapon()
    {
        _lastGunNum = _currentGunNum;
        _currentGunNum = _currentGunNum == _listGun.Count-1 ? 0 : _currentGunNum += 1;
        CurrentWeapon.Unequip();
        UnsubscribeInput();
    }

    public void GetPrevWeapon()
    {
        _lastGunNum = _currentGunNum;
        _currentGunNum = _currentGunNum == 0 ? _listGun.Count-1 : _currentGunNum -= 1;
        CurrentWeapon.Unequip();
        UnsubscribeInput();
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
    }

    public void GetPrevUsedWeapon()
    {
        int temp = _currentGunNum;
        _currentGunNum = _lastGunNum;
        _lastGunNum = temp;
        UnsubscribeInput();
    }
}
