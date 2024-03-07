using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Player")]
    [SerializeField] private Animator animator;
    [SerializeField] private Player player;

    private int attackCount = 0;
    private bool b_IsCompleteAttackAnimation = false;
    void Start()
    {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoveAnimation(player.movingState.GetLastPlayerMoveDirection());
        PlayerDodgeAnimation();
        PlayerAttackAnimation();
    }

    public void PlayerMoveAnimation(Vector3 directionOfPlayerMove)
    {
        if (player.playerStateMachine.currentState == player.idleState)
        {
            animator.SetFloat("Speed", 0f);
        }
        else if (player.playerStateMachine.currentState == player.movingState)
        {
            animator.SetFloat("Speed", 0.5f);
        }
        else if (player.playerStateMachine.currentState == player.sprintState)
        {
            animator.SetFloat("Speed", 1f);
        }
    }
    public void PlayerDodgeAnimation()
    {
        animator.SetBool("isDodge", player.b_IsDodege);

        if (AnimationPlayingCheck(0, 0.95f, "Dodge"))
        {
            animator.SetBool("isDodge", player.b_IsDodege);
        }
    }
    public void PlayerAttackAnimation()
    {
        animator.SetBool("isAttack", player.b_IsAttack);
        if (player.b_IsAttack)
        {// Init
            attackCount = player.offenseState.GetCurrentAttack();

            // Content
            
            if (player.GetPlayerAnimationManager().GetPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
            {
                player.GetPlayerAnimationManager().GetPlayerAnimator().SetBool("Attack1", false);
            }
            if (attackCount >= 2 && AnimationPlayingCheck(0, 0.5f, "Attack1"))
            {
                animator.SetBool("Attack2", true);
            }
            if (attackCount >= 3 && AnimationPlayingCheck(0, 0.5f, "Attack2"))
            {
                animator.SetBool("Attack2", false);
                animator.SetBool("Attack3", true);
            }      
        }
    }
    public void PlayerDieAnimation()
    {
        animator.SetTrigger("isDie");
    }
    public void ResetHitParameter()
    {
        player.b_IsHit = false;
    }
    public bool AnimationPlayingCheck(int currentAnimationLayerNumber, float normalizedTime, string currentAnimationName)
    {
        return animator.GetCurrentAnimatorStateInfo(currentAnimationLayerNumber).normalizedTime > normalizedTime
            && animator.GetCurrentAnimatorStateInfo(currentAnimationLayerNumber).IsName(currentAnimationName);
    }

    public bool CheckCurrentAnimationName(string currentAnimationName)
    {
        return player.GetPlayerAnimationManager().GetPlayerAnimator().GetCurrentAnimatorStateInfo(0).IsName(currentAnimationName);
    }

    // Get Function
    public void ResetAttackParameter(int numOfAttack)
    {
        if (numOfAttack == 3)
        {
            player.b_IsAttack = false;
            player.offenseState.SetAttackCooltime(1);
        }
        animator.SetBool("Attack" + numOfAttack, false);
    }
    public Animator GetPlayerAnimator() { return animator; }
}
