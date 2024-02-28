using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefense : PlayerState
{
    public PlayerDefense(Player player, Status stats, PlayerStateMachine playerStateMachine) : base(player, stats, playerStateMachine)
    { }
    float distanceBetweenCurrentToAfter = 3f;
    public override void Enter()
    {
        player.b_IsBlock = true;
    }
    public override void StateActionUpdate()
    {
        // 보스를 찾고 , 보스와 플레이어 사이의 방향벡터를 얻어낸다
        // 이후 방향벡터를 기반으로 Slerp를 이용해 플레이어 넉백 효과를 구현
        // getHit상태와 연관지어 만약 플레이어가 Defense 상태일 때 GetHit상태로 넘어가면 데미지를 덜 입는걸로 => GetHit에서 previouse State가 Defense일 때 경우로 생각?
        // 그렇다면 Defense상태에서 구현할 내용은 일단 넉백, IsBlock의 활성화
    }

    public override void Exit()
    {
        player.b_IsBlock = false;
    }

    public void OnKnockBackDuringBlocking()
    {
        Vector3 directionVectorBetweenPlayerToBoss = (player.transform.position - player.GetBossComponent().transform.position).normalized; // Direction of Boss To Player for KnockBack direction.

        Vector3 afterKnockBackPosition = directionVectorBetweenPlayerToBoss * 3f;

        player.GetPlayerController().Move(
            Vector3.Lerp(player.transform.position, afterKnockBackPosition, 2f));
    }
}
