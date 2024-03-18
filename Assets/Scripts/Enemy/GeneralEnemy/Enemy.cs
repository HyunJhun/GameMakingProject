using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    public EnemyStateMachine enemyStateMachine { get; set; }
    public EnemyIdle idleState { get; set; }
    public EnemyPatrol patrolState { get; set; }
    public EnemyChase chaseState { get; set; }
    public EnemyReturn returnState { get; set; }
    public EnemyReadyForAttack readyForAttackState { get; set; }
    public EnemyAttack attackState { get; set; }
    public EnemyGetHit getHitState { get; set; }
    public EnemyDie dieState { get; set; }

    [Header("Enemy Information")]
    public float f_enemyPatrolSpeed;
    public float f_enemyChasingSpeed;
    private float f_enemyTurnSpeed;
    private float f_timerForPatrol;
    private Status status;
    private NavMeshAgent enemyAgent;
    private Animator animator;
    private Vector3 initPosition;
    [Header("Reference")]
    [SerializeField] private Player player;
    [SerializeField] private DetectPlayer rangeOfDetectPlayer;
    [SerializeField] private DetectPlayer_AttackRange rangeOfAttack;
    [SerializeField] private GameObject attackRangeBox;
    [Header("LayerMask")]
    [SerializeField] private LayerMask WallLayerMask;

    public bool b_isIdle { get; set; }
    public bool b_isChase { get; set; }
    public bool b_isPatrol { get; set; }
    public bool b_isDie { get; set; }
    public bool b_isAttack { get; set; }
    public bool b_isGetHit { get; set; }

    public float f_patrolLength { get; set; }
    public float f_patrolStopingDistance { get; set; }
    public float f_chaseStopingDistacne { get; set; }
    public float f_attackRoundSpeed { get; set; }
    public float f_chaseMaxDistance { get; set; }
    public float f_patrolMaxDistance { get; set; }
    public float f_attackMoveSpeed { get; set; }
    public List<float> AttackDamageList { get; set; } = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        OnInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Player>().b_IsDie)
        {
            enemyAgent.isStopped = true;
            return;
        }
        enemyStateMachine.currentState.StateActionUpdate();

        if(b_isGetHit)
        {
            enemyStateMachine.ChangeState(getHitState);
            return;
        }
        if(status.getHp() <= 0 && enemyStateMachine.currentState != dieState)
        {
            enemyStateMachine.ChangeState(dieState);
            return;
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
        animator = GetComponent<Animator>();

        enemyStateMachine = new EnemyStateMachine();

        idleState = new EnemyIdle(this, status, enemyStateMachine);
        patrolState = new EnemyPatrol(this, status, enemyStateMachine);
        chaseState = new EnemyChase(this, status, enemyStateMachine);
        returnState = new EnemyReturn(this, status, enemyStateMachine);
        attackState = new EnemyAttack(this, status, enemyStateMachine);
        readyForAttackState = new EnemyReadyForAttack(this, status, enemyStateMachine);
        getHitState = new EnemyGetHit(this, status, enemyStateMachine);
        dieState = new EnemyDie(this, status, enemyStateMachine);

        initPosition = transform.position;

        f_enemyTurnSpeed = 360f;
        f_patrolLength = 10f;
        f_patrolStopingDistance = 1f;
        f_chaseStopingDistacne = 5f;
        f_attackRoundSpeed = 7f;
        f_timerForPatrol = 0f;
        f_patrolMaxDistance = 20f;
        f_chaseMaxDistance = 22f;
        f_attackMoveSpeed = 0.004f;

        AttackDamageList.Add(5f);
        AttackDamageList.Add(7f);
        AttackDamageList.Add(8f);

        b_isChase = false;
        b_isIdle = true;
        b_isPatrol = false;
        b_isDie = false;
        b_isAttack = false;

        SetAgentSpeed(f_enemyPatrolSpeed);
        enemyStateMachine.Initialize(idleState);
        attackRangeBox.GetComponent<AttackRangeCheck>().SetType(1);
    }

    private void getHit()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Enemy")
            && !collision.gameObject.CompareTag("Ground")) // 만약 플레이어나 몬스터가 아닌 대상, 즉 각종 맵 오브젝트들과 부딪힐 시
        {
            f_timerForPatrol = 0f;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if(!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Enemy")
            && !collision.gameObject.CompareTag("Ground")) // 만약 플레이어나 몬스터가 아닌 대상, 즉 각종 맵 오브젝트들과 부딪힐 시
        {
            f_timerForPatrol += Time.deltaTime;
            if(f_timerForPatrol >= 5f)
            {
                if(enemyStateMachine.currentState == patrolState)
                {
                    enemyStateMachine.ChangeState(idleState);
                    return;
                }
            }
        }
    }

    private void OnAnimatorMove()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            transform.position += animator.deltaPosition + transform.forward * f_attackMoveSpeed;
        }
    }

    // Get Function
    public Player GetPlayer() { return player; }
    public Status GetEnemyStatus() { return status; }
    public NavMeshAgent GetEnemyNavMeshAgent() { return enemyAgent; }
    public Animator GetAnimator() { return animator; }
    public Vector3 GetInitPosition() { return initPosition; }
    public LayerMask GetWallLayerMask() { return WallLayerMask; }
    public DetectPlayer GetDetectPlayerRange() { return rangeOfDetectPlayer; }
    public DetectPlayer_AttackRange GetAttackRange() { return rangeOfAttack; }
    public GameObject GetAttackRangeBox() { return attackRangeBox; }
    // Set Function
    public void SetAgentSpeed(float speed) { enemyAgent.speed = speed; }
}
