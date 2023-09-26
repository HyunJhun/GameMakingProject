using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [Header("Reference")]
    PlayerMovementHandler player;
    AttackManager attackManager;
    Animator playerAnimator;
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
        PlayerDodgeAnimation();

    }

    public void PlayerDodgeAnimation()
    {
        playerAnimator.SetBool("isDodge", player.getIsDodge());
        if(playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f
            &&playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("human"))
        {
            Debug.Log("´åÁö ³¡");
            player.setIsDodge(false);
            player.setState(PlayerMovementHandler.PlayerState.Idle);
            playerAnimator.SetBool("isDodge", player.getIsDodge());
        }
    }
    public Animator getPlayerAnimator()
    {
        return playerAnimator;
    }
}
