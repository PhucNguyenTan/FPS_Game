using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Projectile_base : MonoBehaviour
{
    Vector3 _direction;
    Quaternion _rotation;
    float _speed = 0f;
    float _radius;
    float _damage;
    float _maxTimeLife = 30f;
    BoxCollider _box;
    Rigidbody _rb;

    private void Awake()
    {
        
    }

    private void Start()
    {
        Destroy(this.gameObject, _maxTimeLife);
        
    }

    private void Update()
    {
        transform.position += _direction * _speed * Time.deltaTime;
    }

    public Projectile_base AddDirection(Vector3 dir)
    {
        _direction = dir;
        return this;
    }

    public Projectile_base AddShape(GameObject shape)
    {
        Instantiate(shape, transform, false);
        return this;
    }

    public Projectile_base AddSpeed(float speed)
    {
        _speed = speed;
        return this;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

}
