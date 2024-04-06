using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : Enemy
{
    [SerializeField] private EnemyStateMachine skeletonStateMachine { get; set; }

    private void Start()
    {
        OnInitialize();
    }

    private void Update()
    {
        OnBaseUpdate(skeletonStateMachine, enemyAgent);
        skeletonStateMachine.currentState.StateActionUpdate();
    }

    private void FixedUpdate()
    {

    }
    protected override void OnInitialize()
    {
        base.OnInitialize();

        status = GetComponent<Status>();
        enemyAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        skeletonStateMachine = new EnemyStateMachine();

        idleState = new EnemyIdle(this, status, skeletonStateMachine);
        patrolState = new EnemyPatrol(this, status, skeletonStateMachine);
        chaseState = new EnemyChase(this, status, skeletonStateMachine);
        returnState = new EnemyReturn(this, status, skeletonStateMachine);
        attackState = new SkeletonAttackState(this, status, skeletonStateMachine);
        readyForAttackState = new SkeletonReadyForAttackState(this, status, skeletonStateMachine);
        getHitState = new EnemyGetHit(this, status, skeletonStateMachine);
        dieState = new EnemyDie(this, status, skeletonStateMachine);

        initPosition = transform.position;

        // Modifiable
        f_patrolMaxDistance = 20f;
        f_chaseMaxDistance = 22f;
        f_attackMoveSpeed = 0.004f;
        f_patrolMaxTimeForCantMove = 5f;
        f_chaseStopingDistacne = 4f;
        f_attackRoundSpeed = 25f;


        AttackDamageList.Add(5f);
        AttackDamageList.Add(7f);
        AttackDamageList.Add(8f);

        b_isAttack = false;
        b_isCollide = false;

        SetAgentSpeed(f_enemyPatrolSpeed);
        skeletonStateMachine.Initialize(idleState);
        attackRangeBox.GetComponent<AttackRangeCheck>().SetType(1);

        Debug.Log(base.GetEnemyStatus().getHp());

    }
    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Enemy")
            && !collision.gameObject.CompareTag("Ground")) // 만약 플레이어나 몬스터가 아닌 대상, 즉 각종 맵 오브젝트들과 부딪힐 시
        {
            f_timerForPatrol += Time.deltaTime;
            if (f_timerForPatrol >= f_patrolMaxTimeForCantMove)
            {
                if (skeletonStateMachine.currentState == patrolState)
                {
                    skeletonStateMachine.ChangeState(idleState);
                    return;
                }
            }
        }
    }
    private void OnAnimatorMove()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            transform.position += animator.deltaPosition + transform.forward * f_attackMoveSpeed;
        }
    }

}
