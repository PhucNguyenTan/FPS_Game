using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{

    [SerializeField] private Vector3 _currentRotation;
    [SerializeField] private Vector3 TargetRotation;

    [SerializeField] private float RecoilX;
    [SerializeField] private float RecoilY;
    [SerializeField] private float RecoilZ;

    [SerializeField] private float _snappiness;
    [SerializeField] private float _returnSpeed;

    private void Update()
    {
        TargetRotation = Vector3.Lerp(TargetRotation, Vector3.zero, _returnSpeed * Time.deltaTime);
        _currentRotation = Vector3.Slerp(_currentRotation, TargetRotation, _snappiness * Time.fixedDeltaTime);// ???
        transform.localRotation = Quaternion.Euler(_currentRotation);
    }


    public void PistolRecoil()
    {
        TargetRotation = new Vector3(RecoilX, Random.Range(-RecoilY, RecoilY), Random.Range(-RecoilZ, RecoilZ));
    }

    public void ShotgunRecoil()
    {
        TargetRotation = new Vector3(RecoilX, Random.Range(-RecoilY, RecoilY), Random.Range(-RecoilZ, RecoilZ));

    }

    private void OnEnable()
    {
        Gun_Pistol.Shooting += PistolRecoil;
        Gun_Shotgun.Shooting += ShotgunRecoil;
    }

    private void OnDisable()
    {
        Gun_Pistol.Shooting -= PistolRecoil;
        Gun_Shotgun.Shooting -= ShotgunRecoil;
    }
}
