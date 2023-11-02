using System.Collections;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Move,
        Sprint,
        Dodge,
        IdleToDodge,
        Attack
    }
    [Header("Input Property")]
    [SerializeField] private float walkSpeed = 5.0f; // 캐릭터 움직이는 속도
    [SerializeField] private float rotationSpeed = 360f; // 캐릭터가 회전하는 속도
    //dodge
    [SerializeField] private bool isDodge;
    [SerializeField] private float dodgeSpeed = 0.05f;
    [SerializeField] private float dodgeStamina = 10f;
    private PlayerState currentState = PlayerState.Idle;
    [SerializeField] private bool isLockOn;
    private bool isSprint { get; set; } = false;

    [Header("Environment Property")]
    [SerializeField] private float gravity;

    [Header("References")]
    public ThirdPersonCameraHandler cameraHandler;
    [SerializeField] Transform cam;
    [SerializeField] AttackManager attackManager;
    [SerializeField] AnimationManager animationManager;
    [SerializeField] Status stats;
    CharacterController player;

    public bool movecheck = false;
    void Start()
    {
        // Get Component
        player = GetComponent<CharacterController>();
        animationManager = GetComponent<AnimationManager>();
        //dodge
        isDodge = false;
    }

    // Update is called once per frame
    void Update()
    {
        StateUpdate();
        AnimationUpdate();
        LockOnUpdate();
        Debug.Log("Cur = " + currentState);

    }
    private void StateAction()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                return;
            case PlayerState.Move:
                Move();
                return;
            case PlayerState.Attack:
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
                    setState(PlayerState.Idle);
                    return;
                }
                return;
        }
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
                if (attackManager.getIsAttack() == false)
                {
                    setState(PlayerState.Idle);
                    return;
                }
                attackManager.attack(); // 수정 필요
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
        }
        Move();
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

    // Get 함수
    public bool getIsLockOn()
    {
        return isLockOn;
    }

    public bool getIsDodge()
    {
        return isDodge;
    }

    public CharacterController getPlayerController()
    {
        return player;
    }

    public PlayerState GetState()
    {
        return currentState;
    }
    // Set 함

    public void setState(PlayerState state)
    {
        currentState = state;
    }

    public void setIsDodge(bool values)
    {
        isDodge = values;
    }

    public void setPlayerSpeed(float speed)
    {
        walkSpeed = speed;
    }
}


