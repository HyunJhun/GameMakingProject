using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Move,
        Attack
    }
    [Header("Input Property")]
    [SerializeField] private float walkSpeed = 5.0f; // 캐릭터 움직이는 속도
    [SerializeField] private float rotationSpeed = 360f; // 캐릭터가 회전하는 속도
    [SerializeField] private bool isDodge;
    public PlayerState baseState;
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

    void Start()
    {
        player = GetComponent<CharacterController>();
        isDodge = false;
        baseState = PlayerState.Idle;
        animationManager = GetComponent<AnimationManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        if (baseState != PlayerState.Attack) // 공격 도중에는 움직이지 못함
        {
            PlayerMovement(delta);
        }
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

        if (direction.sqrMagnitude > 0.01f) // 캐릭터 이동시 움직이는 형태 관련
        {
            baseState = PlayerState.Move;
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

            // 달리기
            if (Input.GetKeyDown(KeyCode.LeftShift))
            { 
                stats.InvokeRepeating("staminaDecrease", 1f, 1f);
                walkSpeed = 8.0f;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            { 
                stats.InvokeCancle("staminaDecrease");
                walkSpeed = 5.0f;
            }
        }
        if (Input.GetButtonDown("Dodge"))
        {
            dodge();
        }
        OnGravity(direction);
        LockOnChanger();
        animationManager.PlayerAnimation(direction);
        player.Move(moveDir * walkSpeed * delta);
    }

    // 이동에 관한 함수
    private void OnGravity(Vector3 direction)
    {
        direction.y = direction.y - gravity;
    }
    void dodge()
    {
        isDodge = true;
        walkSpeed = 2.5f;

        Invoke("dodgeOut", 0.4f);
    }

    void dodgeOut()
    {
        walkSpeed = 5.0f;
        isDodge = false;
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

    // Set 함
    
}


