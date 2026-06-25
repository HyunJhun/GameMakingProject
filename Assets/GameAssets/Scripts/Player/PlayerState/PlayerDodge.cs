using System.Collections;
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
            player.b_IsDodge = true;
            SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Player.Dodge, false);
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


    public override void Exit()
    {
        player.b_IsDodge = false;
    }

    IEnumerator Dodge()
    {
        // Lccal Var
        float timer;
        float duration = 0.5f;
        Vector3 moveDir;
        // Init
        timer = 0f;
        moveDir = player.movingState.GetLastPlayerMoveDirection().magnitude > 0.001f ?
            player.movingState.GetLastPlayerMoveDirection() : player.transform.forward;


        // Content
        player.GetPlayerStatus().staminaDown_Dodge(player.f_StaminaUsageForDodge);


        player.transform.LookAt(player.transform.position + moveDir);
        while (timer < duration)
        {
            player.GetPlayerController().Move(moveDir * player.f_PlayerDodgeSpeed * Time.deltaTime * player.f_PlayerDodgeDistance);
            timer += Time.deltaTime;
            yield return null;
        }
        playerStateMachine.ChangeState(player.idleState);
    }
}
