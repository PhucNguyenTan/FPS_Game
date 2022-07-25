using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smg_Shoot_spin : MonoBehaviour
{
    [SerializeField] float _spinSpeed;
    [SerializeField] float _spinFriction;
    [SerializeField] float _spinAddVec;
    [SerializeField] float _maxSpin;
    GameObject _spinner;
    float _spinVelocity = 0f;
    private void Awake()
    {
        _spinner = transform.Find("Spinner").gameObject;
    }

    private void Update()
    {
        if (_spinVelocity > 0f)
        {
            _spinner.transform.Rotate(Vector3.up, _spinVelocity * _spinSpeed * Time.deltaTime);
            _spinVelocity -= _spinFriction;
        }
    }

    private void OnEnable()
    {
        Gun_Smg.Shooting += Shoot;
    }
    private void OnDisable()
    {
        Gun_Smg.Shooting -= Shoot;

    }

    void Shoot()
    {
        _spinVelocity += _spinAddVec;
        _spinVelocity = Mathf.Min(_spinVelocity, _maxSpin);
    }

}
