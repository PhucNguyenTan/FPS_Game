using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Projectile_base : MonoBehaviour
{
    Vector3 _direction;
    Quaternion _rotation;
    [SerializeField] Explosion_base _explosion;


    GameObject _shape;
    LayerMask _touchableLayers;
    float _speed;
    float _scale;
    float _maxLifeSpan;
    Explosion_data _explosionData;
    Rigidbody _rb;
    

    bool _isStopMoving = true;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(this.gameObject, _maxLifeSpan);
    }

    private void Update()
    {
        if (!_isStopMoving)
        {
            Vector3 nextPoint = transform.position + _direction * _speed * Time.deltaTime;
            RaycastHit hit;
            bool touched = Physics.Raycast(transform.position, transform.forward, out hit, 1000f, _touchableLayers);
            Debug.DrawLine(transform.position, hit.point);
            if (touched)
            {
                float ab = Vector3.Distance(transform.position, nextPoint);
                float ac = Vector3.Distance(transform.position, hit.point);
                if (ab > ac)
                {
                    nextPoint = hit.point;
                    _isStopMoving = true;
                }
            }
            transform.position = nextPoint;
        }
        transform.localScale = Vector3.one * _scale;
        
    }

    public Projectile_base AddDirection(Vector3 dir)
    {
        _direction = dir;
        return this;
    }

    public Projectile_base CreateShape(GameObject shape)
    {
        _shape = Instantiate(shape, transform, false);
        return this;
    }

    public Projectile_base SetSpeed(float speed)
    {
        _speed = speed;
        return this;
    }

    public Projectile_base SetScale(float scale)
    {
        _scale = scale;
        return this;
    }

    public Projectile_base AddScale(float addScale)
    {
        _scale += addScale;
        return this;
    }

    public Projectile_base SetPosition(Vector3 pos)
    {
        transform.position = pos;
        return this;
    }

    public Projectile_base SetRotation(Quaternion rot)
    {
        transform.rotation = rot;
        return this;
    }

    public Projectile_base Release() {
        _isStopMoving = false;
        return this;
    }

    public Projectile_base SetProjectileData(Projectile_data data)
    {
        _speed = data.Speed;
        _maxLifeSpan = data.MaxLifeSpan;
        _touchableLayers = data.LayerMasks;
        _shape = Instantiate(data.Shape, transform, false);
        _explosionData = data.ExplosionData;
        _scale = data.Scale;
        return this;
    }

    private void OnTriggerEnter(Collider other)
    {
        int objLayer = other.gameObject.layer;
        if ((_touchableLayers & (1 << objLayer)) != 0) // ??? This is bit shift, 1 for exclude, 0 for include depend on where 
        {
            //Generating explode gameobject here
            Instantiate(_explosion, transform.position, Quaternion.identity).SetExplosionData(_explosionData);
            Destroy(this.gameObject);
        }
    }



}
