using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Standard Player State")]
    [SerializeField] private float walkSpeed = 5.0f; // 캐릭터 움직이는 속도
    [SerializeField] private float rotationSpeed = 360f; // 캐릭터가 회전하는 속도

    /*
    [Header("Jump Property")]
    [SerializeField] private float gravitationalAcceleration;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    private GroundCheck groundChecker;
    */
    private enum PlayerState
    {
        idle = 0,
        forward = 1,
        left = 2,
        right = 3,
        backward = 4
    }

    Animator playerAnimator;

    CharacterController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
        playerAnimator = GetComponentInChildren<Animator>();
        //groundChecker = GameObject.Find("GroundChecker").GetComponent<GroundCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal") * walkSpeed, 0, Input.GetAxis("Vertical") * walkSpeed);

        if(direction.sqrMagnitude > 0.01f)
        {
            Vector3 forward = Vector3.Slerp(
                transform.forward,
                direction,
                rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, direction)
                );
            transform.LookAt(transform.position + forward);
        }
        Debug.Log(direction);
        /*
        if (Input.GetButtonDown("Jump") && !groundChecker.IsGrounded())
        {
            direction.y = jumpForce;
            Debug.Log("점프키 입력");
        }
        direction.y = direction.y - gravity;
        */
        if (direction.z > 0) // forward
        {
            playerAnimator.SetInteger("State", (int)PlayerState.forward);
        }
        else if (direction.z < 0) // backward
        {
            playerAnimator.SetInteger("State", (int)PlayerState.backward);
        }
        else if (direction.x > 0) // right
        {
            playerAnimator.SetInteger("State", (int)PlayerState.right);
        }
        else if (direction.x < 0) // left
        {
            playerAnimator.SetInteger("State", (int)PlayerState.left);
        }
        else
            playerAnimator.SetInteger("State", (int)PlayerState.idle);
        player.Move(direction * Time.deltaTime);
    }

}
