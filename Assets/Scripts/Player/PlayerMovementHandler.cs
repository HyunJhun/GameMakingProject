using System.Collections;
using UnityEngine;

/*
 FSM은 항상 한 프레임에 하나의 상태를 선정
즉, 하나의 상태로 변경한 후에는 return 등을 이용해
반환해준 후 다시 상태를 체크하여 정해야함

코드 싹 다 갈아끼우자
지금은 너무 중구난방

입력 처리에 대해서 정리를 하자.
한 곳에서만 이루어져야해(함수마다 어떤걸 키 입력을 통해 받아오고 이런거 다 집어치우고 update문에서 다 처리시켜버려)

역할을 다 분리해야해(역할을 스크립트로 다 나눠)
그래야 필요한걸 바로바로 불러와서 사용하면 되는거야

fsm- 1. 입력처리는 하나의 함수 - 어떤 상태에서 처리해야 될 입력이 무엇인지와 그에 필요한 함수 호출
2. 상태가 변경되었을 때 만약 상태가 변경되지 않고 처리해야할게 있으면 그냥 하고
아니면 꼭 return해서 반환을 시켜주어야 한다.

public은 절대 금. set,get 을 쓰거나 
[serializedField]사용을 권장

컴포넌트는 awake()와 start() 사이 시점에 붙음
awake()에서는 컴포넌트를 붙이지 말아야 함. 보장x

자료형을 var은 auto처럼 자동으로 받아오지만 고정이 되는 
반면, dynamic으로 쓰게되면 후에 변수의 자료형을 변경하게 되면 동적으로 변경이 이루어진다.
 */
public class PlayerMovementHandler : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Move,
        Running,
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
        StateAction();
        AnimationUpdate();
        Debug.Log("Cur = " + currentState);

    }
    private void StateAction()
    {
        LockOnChanger();
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
                        Dodge(true);
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
                        Dodge(false);
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
                    if (stats.getStamina() > 0)
                    {
                        stats.InvokeRepeating("staminaDown_Sprint", 1f, 1f);
                        setPlayerSpeed(8f);
                    }
                }
                if (Input.GetButtonUp("Sprint") || stats.getStamina() <= 0)
                {
                    stats.InvokeCancle("staminaDown_Sprint");
                    setPlayerSpeed(5f);
                }

                if (Input.GetButtonDown("Attack"))
                {
                    setState(PlayerState.Attack);
                    return;
                }
                return;
            case PlayerState.Attack:
                if (Input.GetButtonDown("Dodge"))
                {
                    setState(PlayerState.Dodge);
                    return;
                }
                if(attackManager.getIsAttack() == false)
                {
                    setState(PlayerState.Idle);
                    return;
                }
                return;
            case PlayerState.IdleToDodge:
                return;
            case PlayerState.Dodge:
                return;
        }
    }
    private void AnimationUpdate()
    {
        animationManager.PlayerDodgeAnimation();
    }
    private void Move()
    {
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;
        Vector3 forwardRelatvie = Input.GetAxisRaw("Horizontal") * camRight;
        Vector3 rightRelatvie = Input.GetAxisRaw("Vertical") * camForward;
        
        Vector3 moveDir = (forwardRelatvie + rightRelatvie).normalized; // normalized을 통해 대각선으로 움직여도 값을 1로 맞추어 이동속도가 달라지는 일이 없게 함
        if (isLockOn == false) // Lock Off
        {   // 캐릭터의 회전을 부드럽게 해주는 작업. Slerp를 사용해 구면 회전을 이용하였음
            Vector3 forward = Vector3.Slerp(transform.forward,moveDir,
                rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, moveDir));
            transform.LookAt(transform.position + forward);
        }
        else // Lock On
        {
            Vector3 forward = Vector3.Slerp(transform.forward,cameraHandler.combatLook(),
                rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, cameraHandler.combatLook()));
            transform.LookAt(transform.position + forward);
        }
        animationManager.PlayerMoveAnimation(moveDir);

        if (moveDir.magnitude == 0) // 움직이지 않을 때
        {
            setState(PlayerState.Idle);
            return;
        }
        player.Move(moveDir * walkSpeed * Time.deltaTime);

    }
    private void Dodge(bool isIdle)
    {

        if (isIdle)
            StartCoroutine(IDodge()); // 코루틴 dodge
        else // Idle에서 Dodge를 실행할 때는 따로 입력을 받지 않으므로 그 때 플레이어가 보는 방향에 그대로 굴러가면 됨
            StartCoroutine(SDodge());
        return;
    }

    // 이동에 관한 함수
    IEnumerator SDodge()
    {
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;
        Vector3 forwardRelatvie = Input.GetAxisRaw("Horizontal") * camRight;
        Vector3 rightRelatvie = Input.GetAxisRaw("Vertical") * camForward;

        Vector3 moveDir = (forwardRelatvie + rightRelatvie).normalized;
        transform.LookAt(transform.position + moveDir);
        Debug.Log("ㅇㄻㄴ");
        setIsDodge(true);
        float timer = 0f;
        stats.staminaDown_Dodge(dodgeStamina);
        while (timer < 1f)
        {
            player.Move(moveDir * dodgeSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        setState(PlayerState.Move);
        setIsDodge(false);
    }
    IEnumerator IDodge()
    {
        setIsDodge(true);
        float timer = 0f;
        stats.staminaDown_Dodge(dodgeStamina);
        while (timer < 1f)
        {
            player.Move(transform.forward * dodgeSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        setState(PlayerState.Move);
        setIsDodge(false);
    }
    // 카메라
    private void LockOnChanger()
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


