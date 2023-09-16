using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Forward,
        Left,
        Right,
        Backward,
        Sprint,
        Attack,
        Defense
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

    Animator playerAnimator;
    CharacterController player;

    void Start()
    {
        player = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        baseState = PlayerState.Idle;
        isDodge = false;
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        PlayerMovement(delta);
    }
    private void PlayerAnimation(Vector3 direction)
    {
        if (isLockOn == true)
        {
            playerAnimator.SetLayerWeight(1, 1);
            if (direction.x > 0.5)
            {
                baseState = PlayerState.Right;
                playerAnimator.SetInteger("direction", (int)baseState);
            }
            else if (direction.x < -0.5)
            {
                baseState = PlayerState.Left;
                playerAnimator.SetInteger("direction", (int)baseState);
            }
            else if (direction.z > 0.5)
            {
                baseState = PlayerState.Forward;
                playerAnimator.SetInteger("direction", (int)baseState);
            }
            else if (direction.z < -0.5)
            {
                baseState = PlayerState.Backward;
                playerAnimator.SetInteger("direction", (int)baseState);
            }
            else
            {
                baseState = PlayerState.Idle;
                playerAnimator.SetInteger("direction", (int)baseState);
                Debug.Log("IDLE");
            }
            
            if(Input.GetMouseButtonDown(0))
            {
                //baseState = PlayerState.Attack;
                playerAnimator.SetBool("isAttack", true);
            }
            else if(Input.GetMouseButtonUp(0))
            {
                //baseState = PlayerState.Attack;
                playerAnimator.SetBool("isAttack",false);
            }
        }
        else if (isLockOn == false)
        {
            playerAnimator.SetLayerWeight(1, 0);
            if (direction.magnitude > 0.5) // 플레이어가 움직일 떄
            {
                baseState = PlayerState.Forward;
                playerAnimator.SetInteger("direction", (int)baseState);
            }
            else
            {
                baseState = PlayerState.Idle;
                playerAnimator.SetInteger("direction", (int)baseState);
            }
            if (Input.GetMouseButtonDown(0))
            {
                //baseState = PlayerState.Attack;
                playerAnimator.SetBool("isAttack", true);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                //baseState = PlayerState.Attack;
                playerAnimator.SetBool("isAttack", false);
            }
        }
        playerAnimator.SetBool("isDodge", isDodge);
        
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
            if (isLockOn == false) // Lock On 시
            {//캐릭터가 보고 있는 정면의 방향을 계산해서 보여주는 작업

                Vector3 forward = Vector3.Slerp(
                  transform.forward,
                  moveDir,
                  rotationSpeed * delta / Vector3.Angle(transform.forward, direction)
                  );
                transform.LookAt(transform.position + forward);


            }
            else // Lock Off 시
            {
                Vector3 forward = Vector3.Slerp(
                    transform.forward,
                    cameraHandler.combatLook(),
                    rotationSpeed * delta / Vector3.Angle(transform.forward, cameraHandler.combatLook())
                    );
                transform.LookAt(transform.position + forward);
            };
        }
        if (Input.GetButtonDown("Dodge"))
        {
            dodge();
        }
        OnGravity(direction);
        LockOnChanger();
        PlayerAnimation(moveDir);
        player.Move(moveDir * walkSpeed * delta);
    }

    private void OnGravity(Vector3 direction)
    {
        direction.y = direction.y - gravity;
    }

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

    public bool getIsLockOn()
    {
        return isLockOn;
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
}


