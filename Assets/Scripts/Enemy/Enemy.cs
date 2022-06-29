using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    private float health = 50f;

    private Transform player;

    #region state variable
    public Enemy_state_patrolling statePatrolling { get; private set; }
    private Enemy_state_machine stateMachine;
    #endregion

    private void Awake()
    {
        stateMachine = new Enemy_state_machine();
        statePatrolling = new Enemy_state_patrolling(this, stateMachine, "patrol");
        player = GameObject.Find("Player").gameObject.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    public void ChasePlayer()
    {
        agent.SetDestination(player.position);
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
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
