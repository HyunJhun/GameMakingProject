//using System.Numerics;
using UnityEngine;

public class Player : MonoBehaviour
{
    // State
    public PlayerStateMachine playerStateMachine { get; set; }
    public PlayerIdle idleState { get; set; }
    public PlayerMoving movingState { get; set; }
    public PlayerSprnt sprintState { get; set; }
    public PlayerDodge dodgeState { get; set; }
    public PlayerOffense offenseState { get; set; }
    public PlayerSpellCasting spellCastingState { get; set; }
    public PlayerDefense defenseState { get; set; }
    public PlayerGetHIt getHitState { get; set; }
    public PlayerFloating floatingState { get; set; }
    public PlayerDie dieState { get; set; }

    // Reference Of Player
    [Header("Player Reference")]
    [SerializeField] private GameObject attackCollisionBox;
    private CharacterController playerControllerBody;
    private PlayerAnimationManager playerAnimationManager;
    private GroundChecker playerGroundChecker;
    [SerializeField] private AttackRangeCheck attackRangeBox;
    private SkillManager skillManger;
    private ParticleManager particleManager;
    // Reference of Others
    [Header("Other References")]
    public Transform cam;
    [SerializeField] private Boss bossComponent;
    private KeyInputManager keyInputManager;
    [Header("UI")]
    public GameObject deadUiPrefab;
    // Status
    [Header("Status")]
    private Status stats;

    public float f_StaminaUsageForDodge { get; set; }
    public float f_PlayerWalkSpeed { get; set; }
    public float f_PlayerSprintSpeed { get; set; }
    public float f_PlayerRotationSpeed { get; set; }
    public float f_PlayerDodgeSpeed { get; set; }
    public float f_PlayerDodgeDistance { get; set; }
    public float f_PlayerLastAttackTime { get; set; }
    public float f_PlayerKnockbackPower { get; set; }
    public float f_PlayerMaxHeight { get; set; }
    public float f_PlayerGeneralArmor { get; set; }
    public float f_PlayerDefenseArmor { get; set; }
    public int i_PlayerMaxAttackEnemyCount { get; set; }

    // Environment
    [SerializeField] private float f_Graivty { get; set; }

    // Boolean For Animation
    public bool b_IsDodge { get; set; }
    public bool b_IsAttack { get; set; }
    public bool b_IsBlock { get; set; }
    public bool b_IsSprint { get; set; }
    public bool b_IsFloating { get; set; }
    public bool b_IsHit { get; set; }
    public bool b_IsKnockback { get; set; }
    public bool b_IsDie { get; set; }
    public bool b_isParticleCollision { get; set; }

    // Extra 0630
    public Vector3 currentMoveDirection { get; set; }
    public int currentComboCount = 0;
    
    void Start()
    {
        // Init Player Var
        OnInitialize();
        stats.StaminaIncrease();
    }

    // Update is called once per frame
    void Update()
    {
        Environment();
        if (!b_IsDie)
        {
            if (stats.getHp() <= 0)
            {
                playerStateMachine.ChangeState(dieState);
                return;
            }
            if (b_IsHit)
            {
                if (playerStateMachine.currentState == defenseState)
                {
                    GetPlayerAnimationManager().GetPlayerAnimator().SetTrigger("BlockHit");
                    SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Player.Defense, false);
                    b_IsHit = false;
                    return;
                }
                
                playerStateMachine.ChangeState(getHitState);
                return;
            }
            if (!playerControllerBody.isGrounded && b_IsKnockback)
            {
                playerStateMachine.ChangeState(floatingState);
                return;
            }
        }
        // 수정됨: CameraManager가 없는 Boss Scene에서도 Player 상태와 입력을 계속 갱신
        if (CameraManager.cameraManagerInstance == null ||
            !CameraManager.cameraManagerInstance.isDialogRunning)
        {
            playerStateMachine.currentState.StateActionUpdate();
        }

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

        // Init References
        playerControllerBody = GetComponent<CharacterController>();
        stats = GetComponent<Status>();
        playerAnimationManager = GetComponent<PlayerAnimationManager>();
        playerGroundChecker = GetComponent<GroundChecker>();
        keyInputManager = GetComponent<KeyInputManager>();
        skillManger = GetComponent<SkillManager>();
        particleManager = GetComponent<ParticleManager>();

        // Init state
        idleState = new PlayerIdle(this, stats, playerStateMachine);
        movingState = new PlayerMoving(this, stats, playerStateMachine);
        sprintState = new PlayerSprnt(this, stats, playerStateMachine);
        dodgeState = new PlayerDodge(this, stats, playerStateMachine);
        offenseState = new PlayerOffense(this, stats, playerStateMachine);
        spellCastingState = new PlayerSpellCasting(this, stats, playerStateMachine);
        defenseState = new PlayerDefense(this, stats, playerStateMachine);
        getHitState = new PlayerGetHIt(this, stats, playerStateMachine);
        floatingState = new PlayerFloating(this, stats, playerStateMachine);
        dieState = new PlayerDie(this, stats, playerStateMachine);

        // About Player
        f_PlayerWalkSpeed = 5.0f;
        f_PlayerSprintSpeed = 8.0f;
        f_PlayerRotationSpeed = 360f;
        f_PlayerDodgeSpeed = 0.8f;
        f_PlayerDodgeDistance = 9f;
        f_StaminaUsageForDodge = 20f;
        f_PlayerKnockbackPower = 10f;
        f_PlayerMaxHeight = 4f;
        f_PlayerDefenseArmor = 5f;
        f_PlayerGeneralArmor = 1f;
        i_PlayerMaxAttackEnemyCount = 3;

        currentMoveDirection = transform.forward;
        // About Environment
        f_Graivty = -5f;

        // About Check State
        b_IsDodge = false;
        b_IsAttack = false;
        b_IsBlock = false;
        b_IsFloating = false;
        b_IsSprint = false;
        b_IsDie = false;
        b_IsHit = false;
        b_IsKnockback = false;
        b_isParticleCollision = false;

        playerStateMachine.Initialize(idleState);
        stats.SetArmor(f_PlayerGeneralArmor);
        if(DataManager.dataManagerInstance != null)
            stats.SetHudStatus(DataManager.dataManagerInstance.hpData, DataManager.dataManagerInstance.mpData, DataManager.dataManagerInstance.staminaData);
        attackRangeBox.SetType(0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Attackable")) // 플레이어에게 공격이 가능한 Object (ex. Fireball , Bomb)
        {
            if (!b_IsHit)
            {
                stats.hpDown(15 - stats.GetArmor());
                GetPlayerAnimationManager().GetPlayerAnimator().SetTrigger("GetHit");
                b_IsKnockback = true;
            }
        }
    }

    public void ToDamage(int indexOfAttackMotion)
    {
        

        if (attackRangeBox != null)
        {

            if (attackRangeBox.SelectNearEnemies().Count != 0)
            {
                foreach (var enemy in attackRangeBox.SelectNearEnemies())
                {
                    enemy.GetEnemyStatus().hpDown(stats.GetAttackDamage(indexOfAttackMotion));
                    particleManager.InstanceHitParticle(enemy.triggeredPoint);
                    enemy.b_isGetHit = true;
                }
            }

            //if (attackRangeBox.GetTriggeredEnemyList().Count > 0)
            //{
            //    if (attackRangeBox.GetTriggeredEnemyList().Count > 3)
            //    {
            //        attackRangeBox.selectEnemyByMaxAttackCount();
            //    }
            //    foreach (Enemy enemy in attackRangeBox.GetTriggeredEnemyList())
            //    {
            //        enemy.GetEnemyStatus().hpDown(stats.GetAttackDamage(indexOfAttackMotion));
            //        particleManager.InstanceHitParticle(enemy.triggeredPoint);
            //        enemy.b_isGetHit = true;
            //    }
            //}
            else if (attackRangeBox.GetBoss() != null)
            {
                attackRangeBox.GetBoss().GetStatus().hpDown(stats.GetAttackDamage(indexOfAttackMotion));
                SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Boss.Hit, false);
                particleManager.InstanceHitParticle(attackRangeBox.GetBoss().triggeredPoint);
                attackRangeBox.GetBoss().isGetHit = true;
                //attackRangeBox.ResetBossTriggered();
            }

            if(stats != null)
                stats.staminaDown(stats.GetAttackStamina(indexOfAttackMotion));

            SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Player.Attack, false);
        }

    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Breath"))
        {
            if (b_IsDodge) return;
            if (b_isParticleCollision) return;
            b_isParticleCollision = true;
            stats.hpDown(bossComponent.GetStatus().GetAttackDamage(0) - stats.GetArmor());
            b_IsHit = true;

        }
    }
    //public void OnKnockBackDuringBlocking()
    //{
    //    Vector3 directionVectorBetweenPlayerToBoss = Vector3.forward * -1f; // Direction of Boss To Player for KnockBack direction.

    //    Vector3 afterKnockBackPosition = directionVectorBetweenPlayerToBoss * 1.5f;

    //    GetPlayerController().Move(Vector3.Lerp(transform.position,transform.position - afterKnockBackPosition,Time.deltaTime/10f));
    //}
    // Set Functions



    // Get Functions
    public CharacterController GetPlayerController() { return playerControllerBody; }
    public Status GetPlayerStatus() { return stats; }
    public Transform GetPlayerCamera() { return cam; }
    public GameObject GetPlayerAttackCollisionBox() { return attackCollisionBox; }
    public Boss GetBossComponent() { return bossComponent; }
    public PlayerAnimationManager GetPlayerAnimationManager() { return playerAnimationManager; }
    public GroundChecker GetPlayerGroundChecker() { return playerGroundChecker; }
    public KeyInputManager GetKeyInputManager() { return keyInputManager; }
    public SkillManager GetSkillManger() { return skillManger; }
    public ParticleManager GetParticleManager() { return particleManager; }
    public Vector3 GetMoveToDirection()
    {
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;
        Vector3 forwardRelatvie = Input.GetAxisRaw("Horizontal") * camRight;
        Vector3 rightRelatvie = Input.GetAxisRaw("Vertical") * camForward;

        Vector3 moveDir = (forwardRelatvie + rightRelatvie).normalized; // normalized을 통해 대각선으로 움직여도 값을 1로 맞추어 이동속도가 달라지는 일이 없게 함
        currentMoveDirection = moveDir;
        return currentMoveDirection;
    }
}
