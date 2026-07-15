using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
public class Archer : Enemy
{
    [SerializeField] private EnemyStateMachine archerStateMachine { get; set; }

    [Header("Attack")]
    [SerializeField] private Transform arrowShootingPoint;
    [Header("Effect")]
    [SerializeField] private ParticleSystem arrowParticle;

    private ArrowSpawner arrowSpawner;

    private Transform playerTransform;
    private void Start()
    {
        OnInitialize();
    }
    private void Update()
    {
        OnBaseUpdate(archerStateMachine,enemyAgent);
        archerStateMachine.currentState.StateActionUpdate();
    }
    protected override void OnInitialize()
    {
        base.OnInitialize();

        archerStateMachine = new EnemyStateMachine();

        idleState = new EnemyIdle(this, status, archerStateMachine);
        patrolState = new EnemyPatrol(this, status, archerStateMachine);
        chaseState = new EnemyChase(this, status, archerStateMachine);
        returnState = new EnemyReturn(this, status, archerStateMachine);
        attackState = new ArcherAttackState(this, status, archerStateMachine);
        readyForAttackState = new ArcherReadyForAttackState(this, status, archerStateMachine);
        getHitState = new EnemyGetHit(this, status, archerStateMachine);
        dieState = new EnemyDie(this, status, archerStateMachine);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        arrowSpawner = GetComponent<ArrowSpawner>();
        initPosition = transform.position;

        // Modifiable
        f_patrolMaxDistance = 20f;
        f_chaseMaxDistance = 40f;
        f_attackMoveSpeed = 0.004f;
        f_patrolMaxTimeForCantMove = 5f;
        f_chaseStopingDistacne = 13f;
        f_attackRoundSpeed = 25f;

        b_isAttack = false;
        b_isCollide = false;

        SetAgentSpeed(f_enemyPatrolSpeed);
        archerStateMachine.Initialize(idleState);
        
    }

    //public void InstanceArrowAndParticle()
    //{
    //    GameObject arrowClone = GameObject.Instantiate(arrowPrefab,arrowShootingPoint.transform.position, 
    //        Quaternion.Euler(new Vector3(90,0,0)));

    //    ParticleSystem skillParticle = Instantiate(arrowParticle, arrowShootingPoint.transform.position, transform.rotation);
    //    skillParticle.GetComponent<ParticleSystem>().Play();
    //    SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Enemy.ArcherAttack,this);
    //    Destroy(skillParticle.gameObject, 0.5f);
    //}
    public void InstanceArrowAndParticle()
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned. ReLoae");
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            return;
        }
        Vector3 dir = (playerTransform.position - arrowShootingPoint.position).normalized; // 방향 계산 로직 필요
        arrowSpawner.OnShoot(arrowShootingPoint.position, Quaternion.Euler(90, 0, 0), dir);

        ParticleSystem skillParticle = Instantiate(arrowParticle, arrowShootingPoint.transform.position, transform.rotation);
        skillParticle.Play();
        SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Enemy.ArcherAttack, this);
        Destroy(skillParticle.gameObject, 0.5f);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Enemy")
            && !collision.gameObject.CompareTag("Ground")) // 만약 플레이어나 몬스터가 아닌 대상, 즉 각종 맵 오브젝트들과 부딪힐 시
        {
            f_timerForPatrol += Time.deltaTime;
            if (f_timerForPatrol >= f_patrolMaxTimeForCantMove)
            {
                if (archerStateMachine.currentState == patrolState)
                {
                    archerStateMachine.ChangeState(idleState);
                    return;
                }
            }
        }
    }

}
