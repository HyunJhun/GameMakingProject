using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffense : PlayerState
{
    public PlayerOffense(Player player, Status stats, PlayerStateMachine playerStateMachine) : base(player, stats, playerStateMachine)
    { }

    int currentAttack = 0;
    float attackCooltime = 0.0f;
    public override void Enter()
    {
        player.GetPlayerAttackCollisionBox().SetActive(true);
        player.b_IsAttack = true;
        OnAttack();
    }
    public override void StateActionUpdate()
    {
        if (attackCooltime <= 1) attackCooltime -= Time.deltaTime;

        if (Input.GetButtonDown("Dodge"))
        {
            playerStateMachine.ChangeState(player.dodgeState);
            return;
        }
        if (!player.b_IsAttack)
        {
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                playerStateMachine.ChangeState(player.movingState);
                return;
            }
        }
        if(player.f_PlayerLastAttackTime > 1.0f)
        {
            playerStateMachine.ChangeState(player.idleState);
            return;
        }
        if (Input.GetButtonDown("Attack")) // 한번 클릭시 체크가 안되는것은 상태가 변환했기 때문.
        {
            if(attackCooltime <= 0)
                OnAttack();
        }

    }
    public override void StateActionFixedUpdate()
    {
        if (player.GetPlayerAnimationManager().GetPlayerAnimator().GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            player.GetPlayerController().Move(player.movingState.GetLastPlayerMoveDirection() * 0.1f * Time.fixedDeltaTime);
        }
    }

    public override void Exit()
    {
        player.GetPlayerAttackCollisionBox().SetActive(false);
        player.b_IsAttack = false;
        for(int i = 1; i < 4; i++)
        {
            player.GetPlayerAnimationManager().GetPlayerAnimator().SetBool("Attack" + i, false);
        }
    }

    public void OnAttack()
    {
        // Content

        currentAttack++;
        if (player.b_IsAttack == false) player.b_IsAttack = true;

        if (currentAttack > 3) currentAttack = 1;

        // Reset
        if (player.f_PlayerLastAttackTime > 1.0f) currentAttack = 1;

        // Call Triger;
        //그러니까 지금 애니메이션 상태가 attack1이 아니고 attac2와 attack3 둘다 실행이 안되고있을떄.

        if (!player.GetPlayerAnimationManager().CheckCurrentAnimationName("Attack1") &&
            !player.GetPlayerAnimationManager().CheckCurrentAnimationName("Attack2") &&
            !player.GetPlayerAnimationManager().CheckCurrentAnimationName("Attack3"))
        {
            player.GetPlayerAnimationManager().GetPlayerAnimator().SetBool("Attack1", true);
        }
        // Reset Timer
        player.f_PlayerLastAttackTime = 0f;
        Debug.Log("Last Attack is : " + player.f_PlayerLastAttackTime);

    }

    // Get Function
    public int GetCurrentAttack() { return currentAttack; }
    public void SetAttackCooltime(float time) { attackCooltime = time; }
}
