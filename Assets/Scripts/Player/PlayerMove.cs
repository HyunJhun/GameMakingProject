using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Forward,
        Left,
        Right,
        Backward
    }
    [Header("PlayerState Property")]
    [SerializeField] private float walkSpeed = 5.0f; // 캐릭터 움직이는 속도
    [SerializeField] private float rotationSpeed = 360f; // 캐릭터가 회전하는 속도
    [SerializeField] private PlayerState baseState;
    [SerializeField] private bool isLockOn;
    /*
    [Header("Jump Property")]
    [SerializeField] private float gravitationalAcceleration;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    private GroundCheck groundChecker;
    */
    [Header("Environment Property")]
    [SerializeField] private float gravity;

    Animator playerAnimator;

    CharacterController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        baseState = PlayerState.Idle;
        //groundChecker = GameObject.Find("GroundChecker").GetComponent<GroundCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (direction.sqrMagnitude > 0.01f)
        {
            if (isLockOn == false)
            {//캐릭터가 보고 있는 정면의 방향을 계산해서 보여주는 작업
                Vector3 forward = Vector3.Slerp(
                  transform.forward,
                  direction,
                  rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, direction)
                  );
                transform.LookAt(transform.position + forward);
            }
        }
        OnGravity(direction);
        LockOnCheck();
        PlayerAnimation(direction);
        player.Move(direction * walkSpeed * Time.deltaTime);
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
        }
    }

    private void OnGravity(Vector3 direction)
    {
        direction.y = direction.y - gravity;
    }

    private void LockOnCheck()
    {
        if (Input.GetMouseButton(1))
        {
            isLockOn = true;
            Debug.Log("MOUSE LOCK");
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isLockOn = false;
            Debug.Log("MOUSE DOESNT LOCK");
        }
    }
}


