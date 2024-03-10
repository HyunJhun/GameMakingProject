using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerMovementHandler : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Move,
        Sprint,
        Dodge,
        Floating,
        IdleToDodge,
        Attack,
        Die
    }
    [Header("Input Property")]
    [SerializeField] private float walkSpeed = 5.0f; // 캐릭터 움직이는 속도
    [SerializeField] private float rotationSpeed = 360f; // 캐릭터가 회전하는 속도
    //dodge
    [SerializeField] private bool isDodge;
    [SerializeField] private float dodgeSpeed = 0.05f;
    [SerializeField] private float dodgeStamina = 10f;
    private PlayerState currentState = PlayerState.Idle;
    private PlayerState previousState;
    [SerializeField] private bool isLockOn;
    private bool isSprint { get; set; } = false;
    private bool isFloating { get; set; } = false;

    public bool isDamaged { get; set; } = false;
    public bool isCollisionWithBox { get; set; } = false;

    public bool isDie { get; set; } = false;
    private float isDamgedTimer = 0f;

    [Header("Property")]
    [SerializeField] private float gravity;
    [SerializeField] private float knockbackPower;
    [SerializeField] private float maxHeight;
    [SerializeField] private int degree;
    



    [Header("References")]
    public ThirdPersonCameraHandler cameraHandler;
    [SerializeField] Transform cam;
    [SerializeField] AttackManager attackManager;
    [SerializeField] AnimationManager animationManager;
    [SerializeField] Status stats;
    [SerializeField] Boss boss;
    [SerializeField] GroundChecker groundChecker; 
    CharacterController player;

    public bool movecheck = false;
    void Start()
    {
        // Get Component
        player = GetComponent<CharacterController>();
        animationManager = GetComponent<AnimationManager>();
        //dodge
        isDodge = false;
        //init
        previousState = currentState;
        StaminaCheck();
    }

    // Update is called once per frame
    void Update()
    {
        StateUpdate();
        setSpeedByWeaponState();
        CheckPlayerDie();
        OnGravity();
        AnimationUpdate();
        LockOnUpdate();
        IsDamagedUpdate();
        
    }
    private void StateUpdate()
    {
        switch (currentState)
        {        
            case PlayerState.Idle:
                if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
                {
                    setState(PlayerState.Move);
                    return;
                }
                if (Input.GetButtonDown("Attack"))
                {
                    setState(PlayerState.Attack);
                    return;
                }
                if (Input.GetButtonDown("Dodge"))
                {
                    setState(PlayerState.IdleToDodge);
                    return;
                }
                if(!player.isGrounded)
                {
                    setState(PlayerState.Floating);
                    return;
                }
                return;
            case PlayerState.Move:
                if (Input.GetButtonDown("Dodge"))
                {
                    setState(PlayerState.Dodge);
                    return;
                }
                if (Input.GetButtonDown("Sprint"))
                {
                    setState(PlayerState.Sprint);
                    return;
                }
                if (Input.GetButtonDown("Attack"))
                {
                    setState(PlayerState.Attack);
                    return;
                }
                if (!player.isGrounded)
                {
                    setState(PlayerState.Floating);
                    return;
                }
                Move();
                return;
            case PlayerState.Sprint:
                if (Input.GetButtonDown("Attack"))
                {
                    setState(PlayerState.Attack);
                    return;
                }
                if (Input.GetButtonDown("Dodge"))
                {
                    isSprint = false;
                    stats.InvokeCancel("staminaDown_Sprint");
                    setPlayerSpeed(5f);
                    setState(PlayerState.Dodge);
                    return;
                }
                if (!player.isGrounded)
                {
                    setState(PlayerState.Floating);
                    return;
                }
                if (Input.GetButtonUp("Sprint") || stats.getStamina() <= 0)
                {
                    if (isSprint)
                    {
                        isSprint = false;
                        stats.InvokeCancel("staminaDown_Sprint");
                        setPlayerSpeed(5f);
                    }
                    setState(PlayerState.Move);
                    return;
                }
                Sprint();
                return;
            case PlayerState.Attack:
                
                if (Input.GetButtonDown("Dodge"))
                {
                    setState(PlayerState.Dodge);
                    return;
                }
                if (!player.isGrounded)
                {
                    setState(PlayerState.Floating);
                    return;
                }
                attackManager.attack(); 
                return;
            case PlayerState.IdleToDodge:
                if (stats.getStamina() >= 10)
                {
                    if (!getIsDodge())
                        Dodge();
                }
                else
                {
                    Debug.Log("스태미너 부족");
                    setState(PlayerState.Idle);
                    return;
                }
                if (!player.isGrounded)
                {
                    setState(PlayerState.Floating);
                    return;
                }
                return;
            case PlayerState.Dodge:
                if (stats.getStamina() >= 10)
                {
                    if (!getIsDodge())
                        Dodge();
                }
                else
                {
                    Debug.Log("스태미너 부족");
                    setState(PlayerState.Move);
                    return;
                }
                return;
            case PlayerState.Floating:
                if(player.isGrounded)
                {
                    setState(PlayerState.Idle);
                    return;
                }
                return;
            case PlayerState.Die:
                return; 
        }
    }
    private void AnimationUpdate()
    {
        if (currentState == PlayerState.Idle)
            animationManager.PlayerMoveAnimation();
        else
            animationManager.PlayerMoveAnimation(GetMoveToDirection());
        animationManager.PlayerDodgeAnimation();
    }
    private Vector3 GetMoveToDirection()
    {
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;
        Vector3 forwardRelatvie = Input.GetAxisRaw("Horizontal") * camRight;
        Vector3 rightRelatvie = Input.GetAxisRaw("Vertical") * camForward;

        Vector3 moveDir = (forwardRelatvie + rightRelatvie).normalized; // normalized을 통해 대각선으로 움직여도 값을 1로 맞추어 이동속도가 달라지는 일이 없게 함

        return moveDir;
    }
    private void Move()
    {
        Vector3 moveDir = GetMoveToDirection();
        if (isLockOn == false) // Lock Off
        {   // 캐릭터의 회전을 부드럽게 해주는 작업. Slerp를 사용해 구면 회전을 이용하였음
            Vector3 forward = Vector3.Slerp(transform.forward, moveDir,
                rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, moveDir));
            transform.LookAt(transform.position + forward);
        }
        else // Lock On
        {
            Vector3 forward = Vector3.Slerp(transform.forward, cameraHandler.combatLook(),
                rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, cameraHandler.combatLook()));
            transform.LookAt(transform.position + forward);
        }

        if (moveDir.magnitude == 0) // 움직이지 않을 때
        {
            setState(PlayerState.Idle);
            return;
        }
        player.Move(moveDir * walkSpeed * Time.deltaTime);

    }
    private void Dodge()
    {
        StartCoroutine(EDodge());
        return;
    }
    // 이동에 관한 함수
    IEnumerator EDodge()
    {
        setIsDodge(true);
        float timer = 0f;
        stats.staminaDown_Dodge(dodgeStamina);
        if (currentState == PlayerState.IdleToDodge)
        {
            while (timer < 1f)
            {
                player.Move(transform.forward * dodgeSpeed * Time.deltaTime);
                timer += Time.deltaTime;
                yield return null;
            }
            setState(PlayerState.Idle);
        }
        else
        {
            Vector3 moveDir = GetMoveToDirection();
            transform.LookAt(transform.position + moveDir);
            while (timer < 1f)
            {
                player.Move(moveDir * dodgeSpeed * Time.deltaTime);
                timer += Time.deltaTime;
                yield return null;
            }
            setState(PlayerState.Move);
        }
        setIsDodge(false);
    }
    private void Sprint()
    {
        if (stats.getStamina() > 0)
        {
            if (!isSprint)
            {
                isSprint = true;
                stats.InvokeRepeating("staminaDown_Sprint", 1f, 1f);
                setPlayerSpeed(8f);
            }
            Debug.Log("왜?");
        }
        Move();
    }
    private void OnGravity()
    {
        Vector3 velocity = Vector3.zero;
        velocity.y += gravity;
        player.Move(velocity * Time.deltaTime);
    }
    // 카메라
    private void LockOnUpdate()
    {
        if (Input.GetMouseButtonDown(2))
        {
            if (isLockOn == false)
            {
                isLockOn = true;
                cameraHandler.CurrentStyleChanger();
            }
            else
            {
                isLockOn = false;
                cameraHandler.CurrentStyleChanger();
            }
        }
    }
    private void IsDamagedUpdate()
    {
        if(isDamaged)
        {
            isDamgedTimer += Time.deltaTime;
            if(isDamgedTimer >= 1f)
            {
                isDamgedTimer = 0f;
                isDamaged = false;
            }
        }

    }
    private void CheckPlayerDie()
    {
        if (stats.getHp() <= 0f)
        {
            isDie = true;
            stats.CancelInvoke("staminaUp");
            setState(PlayerState.Die);
            return;
        }
    }
    private void StaminaCheck()
    {
        stats.StaminaIncrease();
    }
    private void setSpeedByWeaponState()
    {
        if (currentState != PlayerState.Sprint)
        {
            if (attackManager.currentWeapon == AttackManager.Weapon.TwoHanded) setPlayerSpeed(3f);
            else setPlayerSpeed(5f);
        }
    }
    // 기타
    public IEnumerator KnockBack(Transform transformForDirectionOfKnockBack) // 넉백을 여기서 말고 그냥 다른데서 처리하는게 나을까? => 날라가는건 플레이어인데 여기서 정의하는게 당연.
    {      
        float timer = 0f;
        bool isReachMaxHeight = false;
        Vector3 getNormalVectorBetweenPlayerToBoss = (player.transform.position - transformForDirectionOfKnockBack.position).normalized; // 날라갈 방향
        Vector3 knockBackDirection =
            new Vector3(knockbackPower * getNormalVectorBetweenPlayerToBoss.x, 45 * Mathf.Deg2Rad + knockbackPower, getNormalVectorBetweenPlayerToBoss.z * knockbackPower);
        isFloating = true;
        while (isFloating)
        {
            timer += Time.deltaTime;
            float height= groundChecker.ShotRayForMaxHeightCheck(); // 공중에 떠있는 동안 플레이어의 높이를 체크

            if (height < maxHeight && !isReachMaxHeight) // 최대 높이를 정해서 사인함수의 형태로 움직이게끔
            {
                player.Move(knockBackDirection * Time.deltaTime);
                yield return null;
            }        
            else
            {
                isReachMaxHeight = true;
                if (player.isGrounded) break;
                player.Move(new Vector3(knockBackDirection.x,0f, knockBackDirection.z)*Time.deltaTime);
                yield return null;
            }  
        }
        isFloating = false;      
    }
    // Trigger, Collision
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Attackable")) // 플레이어에게 공격이 가능한 Object (ex. Fireball , Bomb)
        {
            if (!isDamaged)
            {
                stats.hpDown(15);
                StartCoroutine(KnockBack(collision.gameObject.transform));
            }
        }
    }
    // Get 함수
    public bool getIsLockOn() { return isLockOn; }
    public bool getIsDodge() { return isDodge; }
    public CharacterController getPlayerController() { return player; }
    public PlayerState GetState() { return currentState; }
    public GroundChecker GetGroundChecker() { return groundChecker; }
    public Status GetStats() { return stats; }

    public Boss GetBoss() { return boss; }
    // Set 함수
    public void setState(PlayerState state)
    {
        previousState = currentState;
        currentState = state;
    }
    public void setIsDodge(bool values) { isDodge = values; }
    public void setPlayerSpeed(float speed) { walkSpeed = speed; }
}


