using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
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

    [Header("Reference")]
    PlayerMovementHandler player;
    AttackManager attackManager;
    Animator playerAnimator;
    PlayerState baseState;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMovementHandler>();
        playerAnimator = GetComponent<Animator>();
        attackManager = GetComponent<AttackManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayerAnimation(Vector3 direction)
    {
        if (player.getIsLockOn() == true)
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

            if (Input.GetMouseButtonDown(0))
            {
                attackManager.attack();
                //baseState = PlayerState.Attack;
                playerAnimator.SetBool("isAttack", attackManager.getIsAttack());
            }
        }
        else if (player.getIsLockOn() == false)
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
                attackManager.attack();
                //baseState = PlayerState.Attack;
                playerAnimator.SetBool("isAttack", attackManager.getIsAttack());
                Debug.Log(attackManager.getIsAttack());
            }
        }
        playerAnimator.SetBool("isDodge",player.getIsDodge());

    }

    public Animator getPlayerAnimator()
    {
        return playerAnimator;
    }
}
