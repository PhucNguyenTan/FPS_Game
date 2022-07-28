using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy_shoot_kickback : MonoBehaviour
{
    [SerializeField] Weapon_script _data;

    Vector3 _initialPos;
    Vector3 _initialRot;
    float _timer;
    bool _isShoot = false;

    float _lerpTime;
    Vector3 _targetPos;
    Vector3 _targetRot;

    private void Awake()
    {
        _initialPos = transform.localPosition;
        _initialRot = transform.localRotation.eulerAngles;
    }

    private void Update()
    {
        if (_isShoot)
        {
            _timer += Time.deltaTime;
            _timer = Mathf.Min(_timer, _lerpTime);
            float timerRatio = _timer / _lerpTime;

            float move = _data.KickCurve.Evaluate(timerRatio);
            transform.localPosition = Vector3.Lerp(_initialPos, _initialPos + _targetPos, move);
            Vector3 rotation = Vector3.Lerp(_initialRot, _initialRot + _targetRot, move);
            transform.localRotation = Quaternion.Euler(rotation);

            if (_timer == _lerpTime)
            {
                _isShoot = false;
            }
        }
    }

    private void OnEnable()
    {
        Gun_Energy.Shooting += Kickback;
        Gun_Energy.AlternateShooting += KickbackCharge;
        ResetProperty();

    }

    private void OnDisable()
    {
        Gun_Energy.Shooting -= Kickback;
        Gun_Energy.AlternateShooting -= KickbackCharge;
        
    }

    void Kickback()
    {
        _lerpTime = _data.LerpTime;
        _targetPos = _data.TargetPos;
        _targetRot = _data.TargetRos;
        _timer = 0f;
        _isShoot = true;
    }

    void KickbackCharge()
    {
        _lerpTime = _data.A_LerpTime;
        _targetPos = _data.A_TargetPos;
        _targetRot = _data.A_TargetRos;
        _timer = 0f;
        _isShoot = true;
    }

    void ResetProperty()
    {
        transform.localPosition = _initialPos;
        transform.localRotation = Quaternion.Euler(_initialRot);
        _timer = 0f;
        _isShoot = false;
    }
}
