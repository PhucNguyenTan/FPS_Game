using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damageDealt { get; private set; }
    public float fly_speed = 10f;
    private Rigidbody rigid;

    private bool alreadyCheck = false;
    private Vector3 StartingPoint;
    private Vector3 _target;

    private float test = 0f;

    

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        StartingPoint = transform.position;
    }

    private void Update()
    {
        MoveToTarget(_target);
    }

    public void setDamage(float damage)
    {
        damageDealt = damage;
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;
    }

    private void MoveToTarget(Vector3 target)
    {
        test += Time.deltaTime;
        Vector3 move = Vector3.Lerp(StartingPoint, target, test);
            
        
        
        if(test > 1f){
            Vector3 moremove = (target - StartingPoint).normalized * test * fly_speed; // ???
            move += moremove;
        }
        transform.position = move;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!alreadyCheck)
        {
            if (other.CompareTag("Player"))
            {
                other.gameObject.GetComponent<Player>().TakeDamage(damageDealt);
                Destroy(this.gameObject);
            }
            else if (other.CompareTag("Ground"))
            {
                Destroy(this.gameObject);
            }
            alreadyCheck = true;
        }
    }
}
