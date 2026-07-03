using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerFloating : PlayerState
{
    public PlayerFloating(Player player, Status stats, PlayerStateMachine playerStateMachine) : base(player, stats, playerStateMachine)
    { }
    public override void Enter()
    {
        player.b_IsFloating = true;
        OnKnockBack(player.GetBossComponent().transform,player.GetCancellationTokenOnDestroy()).Forget();
        //player.StartCoroutine(KnockBack(player.GetBossComponent().transform));
    }
    public override void StateActionUpdate()
    {
        if(player.GetPlayerController().isGrounded)
        {
            playerStateMachine.ChangeState(player.idleState);
            return;
        }
    }

    public override void Exit()
    {
        player.b_IsFloating = false;
        player.b_IsKnockback = false;
    }
    public async UniTask OnKnockBack(Transform transformForDirectionOfKnockBack, CancellationToken ct = default)
    {
        bool isReachMaxHeight = false;
        Vector3 getNormalVectorBetweenPlayerToBoss = (player.transform.position - transformForDirectionOfKnockBack.position).normalized; // 날라갈 방향
        Vector3 knockBackDirection =
            new Vector3(player.f_PlayerKnockbackPower * getNormalVectorBetweenPlayerToBoss.x, 45 * Mathf.Deg2Rad + player.f_PlayerKnockbackPower, getNormalVectorBetweenPlayerToBoss.z * player.f_PlayerKnockbackPower);

        // Content
        while (player.b_IsFloating)
        {
            float height = player.GetPlayerGroundChecker().ShotRayForMaxHeightCheck(); // 공중에 떠있는 동안 플레이어의 높이를 체크

            if (height < player.f_PlayerMaxHeight && !isReachMaxHeight) // 최대 높이를 정해서 사인함수의 형태로 움직이게끔
            {
                player.GetPlayerController().Move(knockBackDirection * Time.deltaTime);
                await UniTask.Yield(PlayerLoopTiming.Update, ct);
            }
            else
            {
                isReachMaxHeight = true;
                if (player.GetPlayerController().isGrounded) break;
                player.GetPlayerController().Move(new Vector3(knockBackDirection.x, 0f, knockBackDirection.z) * Time.deltaTime);
                await UniTask.Yield(PlayerLoopTiming.Update, ct);
            }
        }
    }
}
