using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun_shoot_kickback : MonoBehaviour
{
    [Header("Kickback")]
    [SerializeField] float _lerpTime = 0.1f;
    [SerializeField] Vector3 _targetPos = new Vector3(0f, 0f, -0.05f);
    [SerializeField] AnimationCurve _curveShoot;

    Vector3 _initialPos;
    float _timer;
    bool _isShoot = false;

    private void Awake()
    {
        _initialPos = transform.localPosition;
    }

    private void Update()
    {
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
        Gun_Shotgun.Shooting += Kickback;
    }

    private void OnDisable()
    {
        Gun_Shotgun.Shooting -= Kickback;
    }

    void Kickback()
    {
        _timer = _lerpTime - _timer;
        _isShoot = true;
    }
}
