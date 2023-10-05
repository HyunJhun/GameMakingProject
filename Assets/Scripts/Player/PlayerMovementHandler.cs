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
        Attack
    }
    [Header("Input Property")]
    [SerializeField] private float walkSpeed = 5.0f; // 캐릭터 움직이는 속도
    [SerializeField] private float rotationSpeed = 360f; // 캐릭터가 회전하는 속도
    //dodge
    [SerializeField] private bool isDodge;
    [SerializeField] private float dodgeSpeed = 0.05f;
    [SerializeField] private AnimationCurve dodgeCurve;
    float dodgeTimer;
    private bool bugFixed = false;
    PlayerState baseState;
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
    private Vector3 camDir;
    void Start()
    {
        player = GetComponent<CharacterController>();
        baseState = PlayerState.Idle;
        animationManager = GetComponent<AnimationManager>();
        //dodge
        isDodge = false;
        Keyframe dodge_lastFrame = dodgeCurve[dodgeCurve.length - 1];
        dodgeTimer = dodge_lastFrame.time;
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        PlayerMovement(delta);
    }
    private void PlayerMovement(float delta)
    {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        // 카메라 방향에 따라 움직이는거
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        camForward.y = 0;
        camRight.y = 0;
        Vector3 forwardRelatvie = direction.x * camRight;
        Vector3 rightRelatvie = direction.z * camForward;

        Vector3 moveDir = forwardRelatvie + rightRelatvie;
        camDir = moveDir;
        if (direction.sqrMagnitude > 0.01f
           && (baseState != PlayerState.Dodge && baseState != PlayerState.Attack)) // 회전 설정 , 특정 상태에서는 회전 불가하도록 설정
        {
            if (isLockOn == false) // Lock Off
            {//캐릭터가 보고 있는 정면의 방향을 계산해서 보여주는 작업

                Vector3 forward = Vector3.Slerp(
                  transform.forward,
                  moveDir,
                  rotationSpeed * delta / Vector3.Angle(transform.forward, direction)
                  );
                transform.LookAt(transform.position + forward);


            }
            else // Lock On
            {
                Vector3 forward = Vector3.Slerp(
                    transform.forward,
                    cameraHandler.combatLook(),
                    rotationSpeed * delta / Vector3.Angle(transform.forward, cameraHandler.combatLook())
                    );
                transform.LookAt(transform.position + forward);
            };
        }
        // 기본 이동키 이외에 추가로 눌러야 할 입력들
        if (baseState != PlayerState.Attack && baseState != PlayerState.Dodge)
        {
            if (Input.GetButtonDown("Sprint"))
            {
                setState(PlayerState.Running);
                stats.InvokeRepeating("staminaDown", 1f, 1f);
                setPlayerSpeed(8f);
            }
        }
        if (Input.GetButtonUp("Sprint"))
        {
            setState(PlayerState.Move);
            stats.InvokeCancle("staminaDown");
            setPlayerSpeed(5f);
        }
        if (Input.GetButtonDown("Dodge"))
        {
            if (baseState != PlayerState.Dodge)
            {
                StartCoroutine(Dodge());
                Debug.Log("닷지냐");
            }
        }
        OnGravity(direction);
        LockOnChanger();
        animationManager.PlayerAnimation(direction);
        if (baseState != PlayerState.Dodge && baseState != PlayerState.Attack) // 다크소울3 에선 공격,구르기 시 캐릭터 움직임이 정해짐
        {
            player.Move(moveDir * walkSpeed * delta);
        }
        Debug.Log(baseState);
    }
    // 이동에 관한 함수
    private void OnGravity(Vector3 direction)
    {
        direction.y = direction.y - gravity;
    }
    IEnumerator Dodge()
    {
        setIsDodge(true);
        setState(PlayerState.Dodge);
        float timer = 0f;
        while (timer < dodgeTimer)
        {
            Debug.Log("시작햇냐");
            float speed = dodgeCurve.Evaluate(timer);
            Vector3 dodgeMoveDir = (transform.forward * dodgeSpeed);
            player.Move(dodgeMoveDir * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        setIsDodge(false);
        bugFixed = false;
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

    public Vector3 getCamDirection()
    {
        return camDir;
    }

    public PlayerState GetState()
    {
        return baseState;
    }
    // Set 함

    public void setState(PlayerState state)
    {
        baseState = state;
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


