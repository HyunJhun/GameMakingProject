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

    [Header("Basic Value")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 360f;
    public bool isBack { get; set; }
    
    public NavMeshAgent agent { get; set; }

    [Header("References")]
    [SerializeField] private Status stats ;
    public DetectPlayer detectPlayer;
    public GameObject player;
    public GameObject backPoint;
    public TMP_Text stateText;
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


        // 기본 값 처리
        isBack = false;

        bossStateMachine.Initialize(idleState);
    }

    private void Update()
    {
        Debug.Log("이즈백 : " + isBack);
        bossStateMachine.currentState.StateActionUpdate();
        stateText.text = "State : " + bossStateMachine.currentState.ToString();
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
            Debug.Log("발동!");
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
