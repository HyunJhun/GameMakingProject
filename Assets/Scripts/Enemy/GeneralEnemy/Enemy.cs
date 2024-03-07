using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public EnemyStateMachine enemyStateMachine { get; set; }
    public EnemyIdle idleState { get; set; }
    public EnemyPatrol patrolState { get; set; }
    public EnemyDetect detectState { get; set; }
    public EnemyChase chaseState { get; set; }
    public EnemyReturn returnState { get; set; }
    public EnemyAttack attackState { get; set; }
    public EnemyDie dieState { get; set; }

    [Header("Enemy Information")]
    public float f_enemyPatrolSpeed;
    public float f_enemyChasingSpeed;
    private float f_enemyTurnSpeed;
    private Status status;
    private NavMeshAgent enemyAgent;
    private Vector3 initPosition;
    [Header("Reference")]
    [SerializeField] Player player;

    [Header("LayerMask")]
    [SerializeField] private LayerMask WallLayerMask;

    public bool b_isIdle { get; set; }
    public bool b_isChase { get; set; }
    public bool b_isPatrol { get; set; }
    public bool b_isDie { get; set; }

    public float f_patrolLenght { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        OnInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.GetComponent<Player>().b_IsDie)
        {
            enemyStateMachine.currentState.StateActionUpdate();
        }
        else enemyAgent.isStopped = true;

        if(status.getHp() <= 0)
        {
            b_isDie = true;
        }
    }

    private void FixedUpdate()
    {
        if (!player.GetComponent<Player>().b_IsDie)
        {
            enemyStateMachine.currentState.StateActionFixedUpdate();
        }
        else enemyAgent.isStopped = true;
    }
    // Function

    private void OnInitialize()
    {
        status = GetComponent<Status>();
        enemyAgent = GetComponent<NavMeshAgent>();

        enemyStateMachine = new EnemyStateMachine();

        idleState = new EnemyIdle(this, status, enemyStateMachine);
        patrolState = new EnemyPatrol(this, status, enemyStateMachine);
        detectState = new EnemyDetect(this, status, enemyStateMachine);
        chaseState = new EnemyChase(this, status, enemyStateMachine);
        returnState = new EnemyReturn(this, status, enemyStateMachine);
        attackState = new EnemyAttack(this, status, enemyStateMachine);
        dieState = new EnemyDie(this, status, enemyStateMachine);

        initPosition = transform.position;

        f_enemyTurnSpeed = 360f;
        f_patrolLenght = 10f;

        b_isChase = false;
        b_isIdle = true;
        b_isPatrol = false;
        b_isDie = false;

        SetAgentSpeed(f_enemyPatrolSpeed);
        enemyStateMachine.Initialize(idleState);
    }

    // Get Function
    public Status GetEnemyStatus() { return status; }
    public NavMeshAgent GetEnemyNavMeshAgent() { return enemyAgent; }
    public Vector3 GetInitPosition() { return initPosition; }
    public LayerMask GetWallLayerMask() { return WallLayerMask; }
    // Set Function
    public void SetAgentSpeed(float speed) { enemyAgent.speed = speed; }
}
