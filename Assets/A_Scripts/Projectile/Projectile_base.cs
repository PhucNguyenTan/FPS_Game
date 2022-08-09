using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public abstract class Projectile_base : MonoBehaviour
{
    Vector3 _direction;
    Quaternion _rotation;

    GameObject _shape;
    LayerMask _touchableLayers;
    LayerMask _damagable;
    float _speed;
    float _maxLifeSpan;
    float _damage;
    protected float _scale;

    Rigidbody _rb;

    

    bool _isExplosive = false;

    bool _isStopMoving = true;

    protected Vector3 _impactNormal;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(this.gameObject, _maxLifeSpan);
    }

    protected virtual void Update()
    {
        if (!_isStopMoving) MoveProjectileForward();
    }

    public void MoveProjectileForward()
    {
        Vector3 nextPoint = transform.position + _direction * _speed * Time.deltaTime;
        RaycastHit hit;
        bool touched = Physics.Raycast(transform.position, transform.forward, out hit, 1000f, _touchableLayers);
        if (touched)
        {
            float ab = Vector3.Distance(transform.position, nextPoint);
            float ac = Vector3.Distance(transform.position, hit.point);
            if (ab > ac)
            {
                nextPoint = hit.point;
                _isStopMoving = true;
                _impactNormal = hit.normal;
            }
        }
        transform.position = nextPoint;
    }

    public void AddDirection(Vector3 dir)
    {
        _direction = dir;
    }

    public void CreateShape(GameObject shape)
    {
        _shape = Instantiate(shape, transform, false);
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetScale(float scale)
    {
        _scale = scale;
    }

    public void AddScale(float addScale)
    {
        _scale += addScale;
    }

    

    public Projectile_base SetPosition(Vector3 pos)
    {
        transform.position = pos;
        return this;
    }

    public void SetRotation(Quaternion rot)
    {
        transform.rotation = rot;
    }

    public void Release() {
        _isStopMoving = false;
    }

    protected void SetGeneralData(Projectile_data data)
    {
        _damage = data.Damage;
        _speed = data.Speed;
        _maxLifeSpan = data.MaxLifeSpan;
        _touchableLayers = data.Touchable;
        _scale = data.Scale;
        _damagable = data.Damagable;
    }

    public Projectile_base SetProjectileData(Projectile_data data)
    {
        //_explosionData = data.ExplosionData;
        //_bulletImpact = data.ImpactEffect;
        //_isExplosive = data.IsExplosive;
        //_explosion = data.Explosion;
        //_isLevelupAble = data.IsLevelupAble;
        return this;
    }

    private void OnTriggerEnter(Collider other)
    {
        int objLayer = other.gameObject.layer;
        bool isCollided = (_touchableLayers & (1 << objLayer)) != 0;
        bool isCollidWithEnemy = (_damagable & (1 << objLayer)) != 0;

        if (isCollided) 
        {
            if (isCollidWithEnemy)
            {
                other.gameObject.GetComponent<Enemy>().TakeDamage(_damage);
            }
            LogicOnColliding();
            Destroy(this.gameObject);

        }
    }

    protected abstract void LogicOnColliding();
}
