using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{

    // State
    public BossStateMachine bossStateMachine { get; set; }
    public BossIdle idleState { get; set; }
    public BossDetect detectState { get; set; }
    public BossChase chaseState { get; set; }
    public BossAttack attackState { get; set; }
    public BossBack backState { get; set; }
    public BossStiffness stiffnessState { get; set; }
    public BossFlight flightState { get; set; }
    public BossFlyAround flyAroundState { get; set; }
    public BossFlightAttack flightAttackState { get; set; }
    public BossLanding landingState { get; set; }
    public BossDead deadState { get; set; }

    [Header("Basic Value")]
    public float rotationSpeed = 360f;
    private RaycastHit hitObject;
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private LayerMask playerLayerMask;
    private float fireballSpeed = 1800f;
    public bool isDie { get; set; } = false;
    public bool isBack { get; set; }
    public bool isCollision { get; set; }
    public bool isGetHit { get; set; }
    public bool isParticleCollision { get; set; }
    public bool isAttack { get; set; }

    // Attack
    public float coolTime_BreathAttack;
    public float coolTime_RushAttack;

    // Raycast

    public NavMeshAgent agent { get; set; }
    public bool isEnterPhaseTwo { get; set; } = false; // 조건을 만족해도 페이즈가 한 번 넘어갔으면 더이상 넘어가지는 않도록 하는 장치.
                                                       // true 가 되면 페이즈가 한 번 바뀌었던 적이 있다는 소리.

    public Vector3 triggeredPoint { get; set; }
    [Header("References")]
    [SerializeField] private Status stats;
    public DetectPlayer detectPlayer;
    public DetectPlayer_AttackRange detectPlayer_AttackRange;
    public BossAnimationHandler bossAnimationHandler;
    public GameObject player;
    public GameObject backPoint;
    // State 확인용 텍스트 - 후에 지워야함
    public DetectBossCollision bossCollisionBox;
    public List<GameObject> flightPoint;
    [SerializeField] private GameObject bossHeadObj;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private GameObject fireballShotingPoint;
    [SerializeField] private GameObject firebombPrefab;
    [SerializeField] private GameObject firebombDropPoint;
    [SerializeField] private GameObject fireBreathPoint;
    [SerializeField] private GameObject fireBreathParticle;

    public ParticleManager bossParticleManager;

    private void Start()
    {
        // GetComp
        agent = GetComponent<NavMeshAgent>();

        bossStateMachine = new BossStateMachine();

        // State 생성
        idleState = new BossIdle(this, stats, bossStateMachine);
        detectState = new BossDetect(this, stats, bossStateMachine);
        chaseState = new BossChase(this, stats, bossStateMachine);
        attackState = new BossAttack(this, stats, bossStateMachine);
        stiffnessState = new BossStiffness(this, stats, bossStateMachine);
        backState = new BossBack(this, stats, bossStateMachine);
        flightState = new BossFlight(this, stats, bossStateMachine);
        flyAroundState = new BossFlyAround(this, stats, bossStateMachine);
        flightAttackState = new BossFlightAttack(this, stats, bossStateMachine);
        landingState = new BossLanding(this, stats, bossStateMachine);
        deadState = new BossDead(this, stats, bossStateMachine);

        // 기본 값 처리
        isBack = false;
        isCollision = false;
        isGetHit = false;
        isAttack = false;
        bossStateMachine.Initialize(idleState);

        //
        for (int i = 1; i <= 8; i++)
        {
            flightPoint.Add(GameObject.Find("point" + i));
        }

    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.yellow);
        if (!isDie)
        {
            if (!player.GetComponent<Player>().b_IsDie)
            {
                bossStateMachine.currentState.StateActionUpdate();
                bossAnimationHandler.animationUpdate(agent.velocity.magnitude, detectPlayer.isDetectPlayer);
            }
            else agent.isStopped = true;
            if (!isAttack)
            {
                if (isGetHit)
                {
                    bossAnimationHandler.GetBossAnimator().SetTrigger("GetHit");
                    isGetHit = false;

                }
            }
            if (stats.getHp() <= 0)
            {
                bossStateMachine.ChangeState(deadState);
                return;
            }
        }
    }

    public int ShotRay(float attackDistance)
    {
        Debug.DrawRay(transform.position + new Vector3(0f, 1f, 0f), transform.forward * (attackDistance + 2), Color.red);
        if (Physics.Raycast(transform.position + new Vector3(0f, 1f, 0f), transform.forward, out hitObject, attackDistance + 2, wallLayerMask))
        {
            Debug.DrawRay(transform.position + new Vector3(0f, 1f, 0f), transform.forward * (attackDistance + 2), Color.blue);
            if (hitObject.transform.CompareTag("Obstacle") && bossStateMachine.currentState == attackState)
            {
                return (int)hitObject.distance - 1;
            }
        }
        return attackState.distanceOfDestination;
    }

    public void InstanceAndShootFireball()
    {
        Vector3 directionBetweenBossToPlayer = (player.transform.position - transform.position).normalized;
        GameObject fireballClone = GameObject.Instantiate(fireballPrefab, fireballShotingPoint.transform.position, Quaternion.identity);
        fireballClone.GetComponent<Rigidbody>().AddForce(directionBetweenBossToPlayer * fireballSpeed);
    }
    public void InstanceAndDrobFirebomb()
    {
        Vector3 dropDirection = Vector3.down;
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            GameObject fireballClone = GameObject.Instantiate(firebombPrefab, fireballShotingPoint.transform.position, Quaternion.identity);
            fireballClone.GetComponent<Rigidbody>().AddForce(dropDirection);
        }
        else
        {
            GameObject fireballClone = GameObject.Instantiate(fireballPrefab, fireballShotingPoint.transform.position, Quaternion.identity);
            fireballClone.GetComponent<Rigidbody>().AddForce(dropDirection * fireballSpeed);
        }
    }
    public GameObject InstanceFireBraath()
    {
        GameObject fireBreathClone = GameObject.Instantiate(fireBreathParticle, fireBreathPoint.transform.position, 
            Quaternion.Euler(fireBreathParticle.transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y,
            fireBreathParticle.transform.rotation.eulerAngles.z));
        fireBreathClone.transform.SetParent(transform);
        fireBreathClone.GetComponent<ParticleSystem>().Play();
        return fireBreathClone;
    }

    private void FixedUpdate()
    {
        bossStateMachine.currentState.StateActionFixedUpdate();
    }
    private void OnCollisionEnter(Collision collision)
    {
        // 보스가 패턴 1 공격 중 장애물에 부딪혀도 정해진 공격 사거리 만큼 이동하는 것을 막기위해
        // 
        if (collision.gameObject.CompareTag("Obstacle") && bossStateMachine.currentState == attackState)
        {
            isCollision = true;
            bossStateMachine.ChangeState(stiffnessState);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BackPoint"))
        {
            isBack = true;
        }
        if(other.CompareTag("Weapon"))
        {
            triggeredPoint = other.ClosestPoint(transform.position);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BackPoint"))
        {
            isBack = false;
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Skill"))
        {
            if (isParticleCollision) return;
            isParticleCollision = true;
            isGetHit = true;
            stats.hpDown(player.GetComponent<Player>().GetPlayerStatus().GetSkillAttackDamage(0));
        }
    }
    public void BossCollisionBoxActive(bool active)
    {
        bossCollisionBox.gameObject.SetActive(active);
    }
    public void DamagingToPlayer(Transform attackerTransform,float valueOfPlayerHpDown)
    {
        if (!player.GetComponent<Player>().b_IsHit)
        {
            StartCoroutine(player.GetComponent<Player>().floatingState.KnockBack(attackerTransform));
            player.GetComponent<Player>().b_IsHit = true;
            player.GetComponent<Player>().GetPlayerStatus().hpDown(valueOfPlayerHpDown - player.GetComponent<Player>().GetPlayerStatus().GetArmor());
        }
    }
    public bool CheckPlayerCollisionToBoss(float valueOfHpDown)
    {
        if (bossCollisionBox.isCollisionWithPlayer)
        {
            DamagingToPlayer(transform, valueOfHpDown);
            return true;
        }
        else
            return false;
    }

    public bool CheckPlayerDodge()
    {
        return player.GetComponent<Player>().b_IsDodege ? true : false;
    }

    public Status GetStatus()
    {
        return stats;
    }
    public GameObject GetBossHeadObj() { return bossHeadObj; }
}
