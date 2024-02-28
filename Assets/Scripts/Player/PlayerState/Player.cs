using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


    // State
    public PlayerStateMachine playerStateMachine { get; set; }
    public PlayerIdle idleState { get; set; }
    public PlayerMoving movingState { get;set; }
    public PlayerSprnt sprintState { get; set; }
    public PlayerDodge dodgeState { get; set; }
    public PlayerOffense offenseState { get; set; }
    public PlayerDefense defenseState { get; set; }
    public PlayerGetHIt getHitState { get; set; }
    public PlayerFloating floatingState { get; set; }
    public PlayerDie dieState { get; set; }

    // Reference Of Player
    [Header("Player Reference")]   
    [SerializeField] private GameObject attackCollisionBox;
    private CharacterController playerControllerBody;
    private PlayerAnimationManager playerAnimationManager;
    // Reference of Others
    [Header("Other References")]
    public Transform cam;
    [SerializeField] private Boss bossComponent;
    // Status
    [Header("Status")]
    private Status stats;

    public float f_StaminaUsageForDodge { get; set; }
    public float f_PlayerWalkSpeed { get; set; }
    public float f_PlayerSprintSpeed { get; set; }
    public float f_PlayerRotationSpeed { get; set; }
    public float f_PlayerDodgeSpeed { get; set; }
    public float f_PlayerLastAttackTime { get; set; }

    // Environment
    [SerializeField] private float f_Graivty { get; set; }

    // Boolean For Animation
    public bool b_IsDodege { get; set; }
    public bool b_IsAttack { get; set; }
    public bool b_IsBlock { get; set; }
    public bool b_IsSprint { get; set; }
    public bool b_IsFloating { get; set; }
    public bool b_IsDie { get; set; }
    void Start()
    {
        // Init Player Var
        OnInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        Environment();

        if(!playerControllerBody.isGrounded)
        {
            playerStateMachine.ChangeState(floatingState);
        }
        playerStateMachine.currentState.StateActionUpdate();    
    }
    private void FixedUpdate()
    {
        playerStateMachine.currentState.StateActionFixedUpdate();
    }
    // Function of Environment

    private void Environment()
    {
        // On Gravity
        OnGravity();

        // On Player Attack
        f_PlayerLastAttackTime += Time.deltaTime;
    }

    private void OnGravity()
    {
        Vector3 velocity = Vector3.zero;
        velocity.y += f_Graivty;
        playerControllerBody.Move(velocity * Time.deltaTime);
    }

    // Function of Initialize
    private void OnInitialize()
    {
        // Init stateMachine
        playerStateMachine = new PlayerStateMachine();

        // Init state
        idleState = new PlayerIdle(this, stats, playerStateMachine);
        movingState = new PlayerMoving(this, stats, playerStateMachine);
        sprintState = new PlayerSprnt(this, stats, playerStateMachine);
        dodgeState = new PlayerDodge(this, stats, playerStateMachine);
        offenseState = new PlayerOffense(this, stats, playerStateMachine);
        defenseState = new PlayerDefense(this, stats, playerStateMachine);
        getHitState = new PlayerGetHIt(this, stats, playerStateMachine);
        floatingState = new PlayerFloating(this, stats, playerStateMachine);
        dieState = new PlayerDie(this, stats, playerStateMachine);
        // Init References
        playerControllerBody = GetComponent<CharacterController>();
        stats = GetComponent<Status>();
        playerAnimationManager = GetComponent<PlayerAnimationManager>();
        playerStateMachine.Initialize(idleState);
        // About Player
        f_PlayerWalkSpeed = 5.0f;
        f_PlayerSprintSpeed = 8.0f;
        f_PlayerRotationSpeed = 360f;
        f_PlayerDodgeSpeed = 0.8f;
        f_StaminaUsageForDodge = 10f;
        // About Environment
        f_Graivty = -5f;

        // About Boolean
        b_IsDodege = false;
        b_IsAttack = false;
        b_IsBlock = false;
        b_IsFloating = false;
        b_IsSprint = false;
        b_IsDie = false;
    }

    public void ResetIsAttackToFalse()
    {
        b_IsAttack = false;
    }

    // Set Functions



    // Get Functions
    public CharacterController GetPlayerController() { return playerControllerBody; }
    public Status GetPlayerStatus() { return stats;}
    public Transform GetPlayerCamera() { return cam; }
    public GameObject GetPlayerAttackCollisionBox() { return attackCollisionBox; }
    public Boss GetBossComponent() { return bossComponent; }
    public PlayerAnimationManager GetPlayerAnimationManager() { return playerAnimationManager; }
}
