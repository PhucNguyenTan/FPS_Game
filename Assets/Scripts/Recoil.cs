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


    public void RecoilFire()
    {
        TargetRotation = new Vector3(RecoilX, Random.Range(-RecoilY, RecoilY), Random.Range(-RecoilZ, RecoilZ));
    }
}
