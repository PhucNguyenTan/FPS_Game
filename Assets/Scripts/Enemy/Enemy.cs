using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    private float health = 50f;

    private Player player;
    private Projectile current_fireball;
    [SerializeField]
    private GameObject projectile;

    #region state variable
    public Enemy_state_patrolling statePatrolling { get; private set; }
    public Enemy_state_death stateDeath { get; private set; }
    public Enemy_state_attacking stateAttack { get; private set; }
    private Enemy_state_machine stateMachine;

    public bool isReadyToAttack { get; private set; } = true;
    public float timeSinceLastShot { get; private set; } = 4f;
    public float coolDownTime { get; private set; } = 3f;

    #endregion

    private void Awake()
    {
        stateMachine = new Enemy_state_machine();
        statePatrolling = new Enemy_state_patrolling(this, stateMachine, "patrol");
        stateDeath = new Enemy_state_death(this, stateMachine, "death");
        stateAttack = new Enemy_state_attacking(this, stateMachine, "atack");
        player = GameObject.Find("Player").GetComponent<Player>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
    }

    private void Start()
    {
        stateMachine.Initialize(statePatrolling);
    }

    private void Update()
    {
        stateMachine.currentState.Logic();
    }

    public void TakeDamage(float dmg_amount)
    {
        health -= dmg_amount;
        if(health <= 0f)
        {
            stateMachine.ChangeState(stateDeath);
        }
    }

    public void DestroyEnemy(float seconds)
    {
        Destroy(this.gameObject, seconds);
    }

    public void MeleeAttack()
    {

    }

    public void RangedAttack()
    {

    }

    public void SpecialAttack()
    {

    }

    public void CreateProjectile()
    {
        current_fireball = Instantiate(projectile, this.transform.position + Vector3.up*2, Quaternion.identity).GetComponent<Projectile>();
        current_fireball.setDamage(10f);
        current_fireball.SetTarget(player.GetFPScamPosition());
    }

    public void ResetCountDown()
    {
        timeSinceLastShot = 0f;
    }

    public void CountingDown()
    {
        timeSinceLastShot += Time.deltaTime;
    }



    //Test
    public void ReadyYet()
    {
        StartCoroutine(IEcountdown());
    }

    public void notReady()
    {
        //isReadyToAttack = false;
    }

    private IEnumerator IEcountdown()
    {
        yield return new WaitForSeconds(coolDownTime);
        isReadyToAttack = true;


    }
    //test
}
