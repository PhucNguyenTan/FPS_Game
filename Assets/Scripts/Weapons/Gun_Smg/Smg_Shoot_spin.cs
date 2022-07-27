using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smg_Shoot_spin : MonoBehaviour
{
    [Header("Spin")]
    [SerializeField] float _spinSpeed = 5;
    [SerializeField] float _spinFriction = 3;
    [SerializeField] float _spinAddVelocity = 200;
    [SerializeField] float _maxSpin = 1000;

    [Header("Kickback")]
    [SerializeField] float _lerpTime = 0.1f;
    [SerializeField] Vector3 _targetPos = new Vector3(0f, 0f, -0.05f);
    [SerializeField] AnimationCurve _curveShoot;

    Vector3 _initialPos;
    float _timer;
    bool _isShoot = false;


    GameObject _spinner;
    float _spinVelocity = 0f;
    private void Awake()
    {
        _spinner = transform.Find("Spinner").gameObject;
        _initialPos = transform.localPosition;
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
            _timer = Mathf.Min(_timer, _lerpTime);
            float timerRatio = _timer / _lerpTime;

            float move = _curveShoot.Evaluate(timerRatio);
            transform.localPosition = Vector3.Lerp(_initialPos, _initialPos + _targetPos, move);

            if (_timer == _lerpTime)
            {
                _isShoot = false;
            } 
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
        _spinVelocity += _spinAddVelocity;
        _spinVelocity = Mathf.Min(_spinVelocity, _maxSpin);
        _timer = _lerpTime - _timer;
        _isShoot = true;
    }
}
