using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

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

    [Header("Basic Value")]
    public float rotationSpeed = 360f;
    private RaycastHit hitObject;    
    [SerializeField]private LayerMask layerMask;
    private float fireballSpeed = 1800f;
    public bool isBack { get; set; }
    public bool isCollision { get; set; }
    // Raycast
    
    public NavMeshAgent agent { get; set; }
    public bool isEnterPhaseTwo { get; set; } = false; // 조건을 만족해도 페이즈가 한 번 넘어갔으면 더이상 넘어가지는 않도록 하는 장치.
                                                       // true 가 되면 페이즈가 한 번 바뀌었던 적이 있다는 소리.

    [Header("References")]
    [SerializeField] private Status stats ;
    public DetectPlayer detectPlayer;
    public DetectPlayer_AttackRange detectPlayer_AttackRange;
    public BossAnimationHandler bossAnimationHandler;
    public GameObject player;
    public GameObject backPoint;
    // State 확인용 텍스트 - 후에 지워야함
    public TMP_Text stateText;
    public TMP_Text previousStateText;
    public TMP_Text countOfBossFlight;
    public List<GameObject> flightPoint;
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] GameObject fireballShotingPoint;
    [SerializeField] GameObject firebombPrefab;
    [SerializeField] GameObject firebombDropPoint;

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


        // 기본 값 처리
        isBack = false;
        isCollision = false;
        bossStateMachine.Initialize(idleState);

        //
        for (int i = 1; i <= 8; i++)
        {
            flightPoint.Add(GameObject.Find("point" + i));
        }

    }

    private void Update()
    {
        bossStateMachine.currentState.StateActionUpdate();
        bossAnimationHandler.animationUpdate(agent.velocity.magnitude,detectPlayer.isDetectPlayer);
        stateText.text = "State : " + bossStateMachine.currentState.ToString();
        previousStateText.text = "P_State : " + bossStateMachine.previousState.ToString();
        countOfBossFlight.text = flyAroundState.count.ToString(); 
    }

    public int ShotRay(float attackDistance)
    {
        Debug.DrawRay(transform.position + new Vector3(0f,1f,0f), transform.forward * (attackDistance + 2), Color.red);
        if (Physics.Raycast(transform.position + new Vector3(0f, 1f, 0f), transform.forward, out hitObject, attackDistance + 2, layerMask))
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
        GameObject fireballClone = MonoBehaviour.Instantiate(fireballPrefab, fireballShotingPoint.transform.position, Quaternion.identity);
        fireballClone.GetComponent<Rigidbody>().AddForce(directionBetweenBossToPlayer * fireballSpeed);
    }
    public void InstanceAndDrobFirebomb()
    {
        Vector3 dropDirection = Vector3.down;
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            GameObject fireballClone = MonoBehaviour.Instantiate(firebombPrefab, fireballShotingPoint.transform.position, Quaternion.identity);
            fireballClone.GetComponent<Rigidbody>().AddForce(dropDirection);
        }
        else
        {
            GameObject fireballClone = MonoBehaviour.Instantiate(fireballPrefab, fireballShotingPoint.transform.position, Quaternion.identity);
            fireballClone.GetComponent<Rigidbody>().AddForce(dropDirection * fireballSpeed);
        }
    }
    private void FixedUpdate()
    {
        bossStateMachine.currentState.StateActionFixedUpdate();
    }
    private void OnCollisionEnter(Collision collision)
    {
        // 보스가 패턴 1 공격 중 장애물에 부딪혀도 정해진 공격 사거리 만큼 이동하는 것을 막기위해
        // 
        if(collision.gameObject.CompareTag("Obstacle") && bossStateMachine.currentState == attackState)
        {
            isCollision = true;
            bossStateMachine.ChangeState(stiffnessState);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BackPoint"))
        {
            isBack = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BackPoint"))
        {
            isBack = false;
        }
    }
}
