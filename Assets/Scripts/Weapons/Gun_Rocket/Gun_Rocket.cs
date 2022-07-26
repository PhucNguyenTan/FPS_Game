using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Rocket : Gun_base
{
    public Gun_Rocket()
    {

    }

    private void OnEnable()
    {
        InputHandler.Instance.SingleShoot += Shoot;
    }

    private void OnDisable()
    {
        InputHandler.Instance.SingleShoot -= Shoot;

    }
    public void Shoot()
    {
        
    }
}
