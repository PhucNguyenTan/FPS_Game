using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smg_Shoot_spin : MonoBehaviour
{
    [SerializeField] float _spinSpeed;
    [SerializeField] float _spinFriction;
    [SerializeField] float _spinAddVec;
    [SerializeField] float _maxSpin;
    [SerializeField] AnimationCurve _curveShoot;


    [SerializeField] float _lerpTime;
    [SerializeField] Vector3 _targetPosSet;

    Vector3 _targetPos = Vector3.zero;
    Vector3 _initialPos;
    float _zInitial;
    float _timer;
    bool _isShoot = false;


    GameObject _spinner;
    float _spinVelocity = 0f;
    private void Awake()
    {
        _spinner = transform.Find("Spinner").gameObject;
        _initialPos = transform.position;
        _zInitial = _initialPos.z;
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
            transform.position = Vector3.Lerp(_initialPos, _targetPos, move);

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
        _spinVelocity += _spinAddVec;
        _spinVelocity = Mathf.Min(_spinVelocity, _maxSpin);
        _timer = _lerpTime - _timer;
        _isShoot = true;
    }
}
