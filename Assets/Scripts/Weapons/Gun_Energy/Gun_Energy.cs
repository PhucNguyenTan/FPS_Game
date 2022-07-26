using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun_Energy : Gun_base
{

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

    public void ShootEnergy()
    {

    }

    public  void ShootSmall()
    {
        
    }

    public void CancelCharge()
    {

    }

    public void Charging()
    {

    }
}
