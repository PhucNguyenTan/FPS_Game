using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Gun_Energy : Gun_base
{

    public static UnityAction Shooting;
    public static UnityAction AlternateShooting;

    public Gun_Energy()
    {
        
    }
    private void OnEnable()
    {
        InputHandler.Instance.HybridCharge += Charging;
        InputHandler.Instance.HybridShoot += ShootSmall;
        InputHandler.Instance.HybridChargedShoot += ShootEnergy;
        InputHandler.Instance.HybridCancel += CancelCharge;
    }

    private void OnDisable()
    {
        InputHandler.Instance.HybridCharge -= Charging;
        InputHandler.Instance.HybridShoot -= ShootSmall;
        InputHandler.Instance.HybridChargedShoot -= ShootEnergy;
        InputHandler.Instance.HybridCancel -= CancelCharge;
    }

    void ShootEnergy()
    {
        if (!CheckCanShoot()) return;
        AlternateShooting?.Invoke();
    }

    void ShootSmall()
    {
        if (!CheckCanShoot()) return;
        Shooting?.Invoke();
    }

    public void CancelCharge()
    {

    }

    public void Charging()
    {

    }
}
