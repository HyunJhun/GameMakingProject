using System.Collections;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;

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
            Dodge(player.GetCancellationTokenOnDestroy()).Forget();
            // cancelllationToken 은 MonoBehavior에 들어있음. 그래서 지금 상황이면 player가 파괴디면 token이 넘어가서 취소되는 거
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

    private async UniTask Dodge(CancellationToken ct = default)
    {
        // Init
        float duration = 0.5f;

        Vector3 moveDir = player.currentMoveDirection.magnitude > 0.001f
            ? player.currentMoveDirection
            : player.transform.forward;

        // Content
        player.GetPlayerStatus().staminaDown_Dodge(player.f_StaminaUsageForDodge);
        player.transform.LookAt(player.transform.position + moveDir);
        // Sound
        SoundManager.soundManagerInstacne.PlaySfx(SoundManager.SFX_Player.Dodge, false);

        float timer = 0f;
        while (timer < duration)
        {
            player.GetPlayerController().Move(moveDir * player.f_PlayerDodgeSpeed * Time.deltaTime * player.f_PlayerDodgeDistance);
            timer += Time.deltaTime;
            await UniTask.Yield(PlayerLoopTiming.Update, ct);
        }

        playerStateMachine.ChangeState(player.idleState);
    }
}
