using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun_shoot_kickback : MonoBehaviour
{
    [SerializeField] Weapon_script _data;
    

    Vector3 _initialPos;
    Vector3 _initialRot;
    float _timer;
    bool _isShoot = false;

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
        Gun_Shotgun.Shooting += Kickback;
        ResetProperty();
    }

    private void OnDisable()
    {
        Gun_Shotgun.Shooting -= Kickback;
    }

    void Kickback()
    {
        _timer = _data.LerpTime - _timer;
        _isShoot = true;
    }

    private void ResetProperty()
    {
        transform.localPosition = _initialPos;
        transform.localRotation = Quaternion.Euler(_initialRot);
        _timer = _data.LerpTime;
        _isShoot = false;
    }
}
