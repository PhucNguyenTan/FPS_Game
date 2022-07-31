using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Smg_Shoot_spin : MonoBehaviour
{
    [Header("Spin")]
    [SerializeField] float _spinSpeed = 5;
    [SerializeField] float _spinFriction = 3;
    [SerializeField] float _spinAddVelocity = 200;
    [SerializeField] float _maxSpin = 1000;

    [Header("Kickback")]                                     
    [SerializeField] Weapon_script _data;

    Vector3 _initialPos;
    Vector3 _initialRot;

    float _timer;
    bool _isShoot = false;


    GameObject _spinner;
    float _spinVelocity = 0f;
    private void Awake()
    {
        _initialPos = transform.localPosition;
        _initialRot = transform.localRotation.eulerAngles;
        _spinner = transform.Find("Spinner").gameObject;
    }


    private void Update()
    {
        if (_spinVelocity > 0f)
        {
            _spinner.transform.Rotate(Vector3.up, _spinVelocity * _spinSpeed * Time.deltaTime);
            _spinVelocity -= _spinFriction;
        }
        if (_isShoot)
        {
            _timer += Time.deltaTime;
            _timer = Mathf.Min(_timer, _data.LerpTime);
            float timerRatio = _timer / _data.LerpTime;

            float move = _data.KickCurve.Evaluate(timerRatio);
            transform.localPosition = Vector3.Lerp(_initialPos, _initialPos + _data.TargetPos, move);
            Vector3 rotation = Vector3.Lerp(_initialRot, _initialRot + _data.TargetRos, move);
            transform.localRotation = Quaternion.Euler(rotation);

            if (_timer == _data.LerpTime)
            {
                _isShoot = false;
            }
        }
    }

    private void OnEnable()
    {
        Gun_Smg.Shooting += Shoot;
        ResetProperty();
    }
    private void OnDisable()
    {
        Gun_Smg.Shooting -= Shoot;

    }

    void Shoot()
    {
        _spinVelocity += _spinAddVelocity;
        _spinVelocity = Mathf.Min(_spinVelocity, _maxSpin);
        _timer = _data.LerpTime - _timer;
        _isShoot = true;
    }

    private void ResetProperty()
    {
        transform.localPosition = _initialPos;
        transform.localRotation = Quaternion.Euler(_initialRot);
        _timer = _data.LerpTime;
        _spinVelocity = 0f;
        _isShoot = false;
    }
}
