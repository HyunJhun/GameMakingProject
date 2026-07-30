using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    #region State
    public EnemyIdle idleState { get; set; }
    public EnemyPatrol patrolState { get; set; }
    public EnemyChase chaseState { get; set; }
    public EnemyReturn returnState { get; set; }
    public EnemyReadyForAttack readyForAttackState { get; set; }
    public EnemyAttack attackState { get; set; }
    public EnemyGetHit getHitState { get; set; }
    public EnemyDie dieState { get; set; }
    #endregion

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
    public EnemyHUD enemyHud { get; set; }
    [Header("LayerMask")]
    [SerializeField] private LayerMask WallLayerMask;
    public Vector3 triggeredPoint { get; set; }
    #region flags
    public bool b_isAttack { get; set; }
    public bool b_isGetHit { get; set; }
    public bool b_isCollide { get; set; }
    public bool b_isDie { get; set; }
    #endregion
    #region variables
    public float f_patrolLength { get; set; }
    public float f_patrolStopingDistance { get; set; }
    public float f_chaseStopingDistacne { get; set; }
    public float f_attackRoundSpeed { get; set; }
    public float f_chaseMaxDistance { get; set; }
    public float f_patrolMaxDistance { get; set; }
    public float f_attackMoveSpeed { get; set; }
    public float f_patrolMaxTimeForCantMove { get; set; }
    public float f_timerForPatrol { get; set; }
    #endregion
    public List<float> AttackDamageList { get; set; } = new List<float>();

    public event Action<Enemy> OnDied;
    private bool hasNotifiedDeath;

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
        enemyHud = GetComponent<EnemyHUD>();
        // Fixed
        f_patrolLength = 10f;
        f_timerForPatrol = 0f;
        f_patrolStopingDistance = 1f;
        b_isDie = false;
        hasNotifiedDeath = false;
    }


    public void ToDamage(int indexOfAttackMotion)
    {
        SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Enemy.SkeletonAttack,this);
        if (attackRangeBox.GetComponent<AttackRangeCheck>().getStats() == null) return;

        // 1. 보스는 애니메이션만 취하고 체력만 깎이면 됨
        // 2. 일반몹은 플레이어가 공격 시 GetHit 상태로 진입하며, 공격 도중이어도 피격 애니메이션으로 전환한다.
        // 3. 플레이어는 공격 시 스태미너가 소모되고, 피격 시에는 공격할 수 없게 된다.
        player.b_IsHit = true;
        attackRangeBox.GetComponent<AttackRangeCheck>().getStats().hpDown(status.GetAttackDamage(indexOfAttackMotion) - player.GetPlayerStatus().GetArmor());
        attackRangeBox.GetComponent<AttackRangeCheck>().ResetTriggerObj();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Enemy")
            && !collision.gameObject.CompareTag("Ground")) // 플레이어나 몬스터가 아닌 맵 오브젝트와 충돌했을 때
        {
            f_timerForPatrol = 0f;     
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Weapon")) // 留뚯빟 ?뚮젅?댁뼱??臾닿린??遺?ろ삍????
        if(other.gameObject.CompareTag("Weapon")) // 플레이어의 무기와 충돌했을 때
            triggeredPoint = other.ClosestPoint(transform.position);

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

    private void OnDisable()
    {
        NotifyDeath();
    }
    public void NotifyDeath()
    {
        if (hasNotifiedDeath)
            return;

        hasNotifiedDeath = true;
        Debug.Log("[Event] Enemy Died");
        OnDied?.Invoke(this);
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
