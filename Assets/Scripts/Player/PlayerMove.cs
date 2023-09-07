using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Standard Player State")]
    [SerializeField] private float walkSpeed = 5.0f; // 캐릭터 움직이는 속도
    [SerializeField] private float rotationSpeed = 360f; // 캐릭터가 회전하는 속도

    [Header("Jump Property")]
    [SerializeField] private float gravitationalAcceleration;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    private GroundCheck groundChecker;
    

    CharacterController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
        groundChecker = GameObject.Find("GroundChecker").GetComponent<GroundCheck>();
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
        if (Input.GetButtonDown("Jump") && !groundChecker.IsGrounded())
        {
            direction.y = jumpForce;
            Debug.Log("점프키 입력");
        }
        direction.y = direction.y - gravity;
        player.Move(direction * Time.deltaTime);
    }

}
