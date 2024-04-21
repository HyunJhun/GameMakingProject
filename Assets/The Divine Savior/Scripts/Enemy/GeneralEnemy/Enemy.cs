using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
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
    
    protected Status status;
    protected NavMeshAgent enemyAgent;
    protected Animator animator;
    protected Vector3 initPosition;
    [Header("Reference")]
    [SerializeField] protected Player player;
    [SerializeField] protected DetectPlayer rangeOfDetectPlayer;
    [SerializeField] protected DetectPlayer_AttackRange rangeOfAttack;
    [SerializeField] protected GameObject attackRangeBox;
    [Header("LayerMask")]
    [SerializeField] private LayerMask WallLayerMask;
    public Vector3 triggeredPoint { get; set; }
    public bool b_isAttack { get; set; }
    public bool b_isGetHit { get; set; }
    public bool b_isCollide { get; set; }
    public bool b_isDie { get; set; }
    public float f_patrolLength { get; set; }
    public float f_patrolStopingDistance { get; set; }
    public float f_chaseStopingDistacne { get; set; }
    public float f_attackRoundSpeed { get; set; }
    public float f_chaseMaxDistance { get; set; }
    public float f_patrolMaxDistance { get; set; }
    public float f_attackMoveSpeed { get; set; }
    public float f_patrolMaxTimeForCantMove { get; set; }
    public float f_timerForPatrol { get; set; }
    public List<float> AttackDamageList { get; set; } = new List<float>();

    // Start is called before the first frame update
    void Start()
    {
        OnInitialize();
    }
    // Function
    protected virtual void OnBaseUpdate(EnemyStateMachine enemyStateMachine,NavMeshAgent enemyAgent)
    {
        if (player.GetComponent<Player>().b_IsDie)
        {
            enemyAgent.isStopped = true;
            return;
        }
        if (b_isGetHit)
        {
            enemyStateMachine.ChangeState(getHitState);
            return;
        }
        if (status.getHp() <= 0 && enemyStateMachine.currentState != dieState)
        {
            enemyStateMachine.ChangeState(dieState);
            return;
        }
    }

    protected virtual void OnInitialize()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        status = GetComponent<Status>();
        enemyAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        // Fixed
        f_patrolLength = 10f;
        f_timerForPatrol = 0f;
        f_patrolStopingDistance = 1f;
        b_isDie = false;
    }


    public void ToDamage(int indexOfAttackMotion)
    {
        if (attackRangeBox.GetComponent<AttackRangeCheck>().getStats() == null) return;

        // 1. 보스는 애니메이션만 취하고 체력만 깎이면 됨
        // 2. 일반몹은 플레이어가 공격시 GetHit 상태로 진입해야 하며 이 때, 공격을 하던 도중이여도 진입을 하게 되므로 강제로 애니메이션을 멈춰주어야 한다.
        // 3. 플레이어는 공격시 스태미너가 소모되야하는 추가적인 작업이 필요하다. 또한 플레이어는 피격시 공격을 할 수 없게 된다.

        player.b_IsHit = true;
        attackRangeBox.GetComponent<AttackRangeCheck>().getStats().hpDown(status.GetAttackDamage(indexOfAttackMotion) - player.GetPlayerStatus().GetArmor());
        attackRangeBox.GetComponent<AttackRangeCheck>().ResetTriggerObj();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Enemy")
            && !collision.gameObject.CompareTag("Ground")) // 만약 플레이어나 몬스터가 아닌 대상, 즉 각종 맵 오브젝트들과 부딪힐 시
        {
            f_timerForPatrol = 0f;     
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Weapon")) // 만약 플레이어의 무기에 부딪혔을 시
        {
            triggeredPoint = other.ClosestPoint(transform.position);
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Skill"))
        {
            if (b_isCollide) return;
            b_isCollide = true;
            b_isGetHit = true;
            status.hpDown(player.GetPlayerStatus().GetSkillAttackDamage(0));
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
