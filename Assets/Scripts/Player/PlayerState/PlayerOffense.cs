using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOffense : PlayerState
{
    public PlayerOffense(Player player, Status stats, PlayerStateMachine playerStateMachine) : base(player, stats, playerStateMachine)
    { }

    int currentAttack = 0;

    public override void Enter()
    {
        player.GetPlayerAttackCollisionBox().SetActive(true);
        OnAttack();
    }
    public override void StateActionUpdate()
    {
        if (Input.GetButtonDown("Dodge"))
        {
            playerStateMachine.ChangeState(player.dodgeState);
            return;
        }
        if (!player.b_IsAttack)
        {
            if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
            {
                playerStateMachine.ChangeState(player.movingState);
                return;
            }
        }
        if(player.f_PlayerLastAttackTime > 1.0f && player.b_IsAttack)
        {
            playerStateMachine.ChangeState(player.idleState);
            return;
        }
        if (Input.GetButtonDown("Attack")) // 한번 클릭시 체크가 안되는것은 상태가 변환했기 때문.
        {
            OnAttack();
        }

    }
    public override void StateActionFixedUpdate()
    {
        base.StateActionFixedUpdate();
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
        player.b_IsAttack = true;

        if (currentAttack > 3)
            currentAttack = 1;

        // Reset
        if (player.f_PlayerLastAttackTime > 1.0f)
            currentAttack = 1;

        // Call Triger;
        if (!player.GetPlayerAnimationManager().GetPlayerAnimator().GetBool("Attack1"))
        {
            player.GetPlayerAnimationManager().GetPlayerAnimator().SetBool("Attack1", true);
        }
        // Player Move During Attack
        //player.StartCoroutine(MoveToAttackForward());
        // Reset Timer
        player.f_PlayerLastAttackTime = 0f;
        Debug.Log("Last Attack is : " + currentAttack);

    }

    public void OnDamage(int indexOfAttackMotion)
    {
        player.GetPlayerStatus().staminaDown(player.GetPlayerStatus().GetAttackStamina(indexOfAttackMotion)); // 공격 스태미너 감소
        if(player.GetPlayerAttackCollisionBox().GetComponent<AttackRangeCheck>().getStats() != null) // 적이 있다면
        {
            if (player.GetBossComponent().isDie == false)
            {
                Status statusOfInRangeObject = player.GetPlayerAttackCollisionBox().GetComponent<AttackRangeCheck>().getStats();
                statusOfInRangeObject.hpDown(player.GetPlayerStatus().GetAttackDamage(indexOfAttackMotion));
                // 보스 맞는 애니메이션 추가.
            }
        }
    }
    IEnumerator MoveToAttackForward()
    {
        yield return null;
        float timer = 0f;
        while (timer < 3f)
        {
            timer += Time.deltaTime;
            player.GetPlayerController().Move(player.transform.forward.normalized * 1f * Time.fixedDeltaTime); 
        }
    }

    // Get Function
    public int GetCurrentAttack() { return currentAttack; }
}
