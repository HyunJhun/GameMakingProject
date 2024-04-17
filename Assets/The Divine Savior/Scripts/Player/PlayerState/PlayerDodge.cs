using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodge : PlayerState
{
    // Start is called before the first frame update
    public PlayerDodge(Player player, Status stats, PlayerStateMachine playerStateMachine) : base(player, stats, playerStateMachine)
    { }

    public override void Enter()
    {
        if (player.GetPlayerStatus() == null) return;

        if (player.GetPlayerStatus().getStamina() >= 10)
        {
            player.b_IsDodege = true;
            player.StartCoroutine(Dodge());
        }
        else
        {
            playerStateMachine.ChangeState(player.idleState);
        }
    }
    public override void StateActionUpdate()
    {

    }
    public override void StateActionFixedUpdate()
    {
    }

    public override void Exit()
    {
        player.b_IsDodege = false;
    }

    IEnumerator Dodge()
    {
        // Lccal Var
        float timer;
        Vector3 moveDir;
        // Init
        timer = 0f;
        moveDir = player.movingState.GetLastPlayerMoveDirection();
        // Content
        player.GetPlayerStatus().staminaDown_Dodge(player.f_StaminaUsageForDodge);

        if(playerStateMachine.previousState == player.idleState)
        {
            while (timer < 0.5f)
            {
                player.GetPlayerController().Move(player.transform.forward * player.f_PlayerDodgeSpeed * Time.deltaTime * player.f_PlayerDodgeDistance);
                timer += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            player.transform.LookAt(player.transform.position + moveDir);
            while (timer < 0.5f)
            {
                player.GetPlayerController().Move(moveDir * player.f_PlayerDodgeSpeed * Time.deltaTime * player.f_PlayerDodgeDistance);
                timer += Time.deltaTime;
                yield return null;
            }
        }
        playerStateMachine.ChangeState(player.idleState);
    }
}
