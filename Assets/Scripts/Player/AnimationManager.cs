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
            playerAnimator.SetFloat("speedX", direction.x);
            playerAnimator.SetFloat("speedY", direction.z);

        }
        else if (player.getIsLockOn() == false)
        {
            playerAnimator.SetLayerWeight(1, 0);
            playerAnimator.SetFloat("speedY", direction.magnitude);
        }
        playerAnimator.SetBool("isDodge",player.getIsDodge());

    }

    public Animator getPlayerAnimator()
    {
        return playerAnimator;
    }
}
