using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetHIt : PlayerState
{
    public PlayerGetHIt(Player player, Status stats, PlayerStateMachine playerStateMachine) : base(player, stats, playerStateMachine)
    { }

    public override void Enter()
    {
        player.GetPlayerAnimationManager().GetPlayerAnimator().SetTrigger("GetHit");
        player.b_IsHit = false;
    }
    public override void StateActionUpdate()
    {
        if(player.b_IsKnockback)
        {
            playerStateMachine.ChangeState(player.floatingState);
            return;
        }
        if (player.GetPlayerAnimationManager().AnimationPlayingCheck(0, 0.6f, "GetHit"))
        {
            playerStateMachine.ChangeState(player.idleState);
            return;
        }
    }
    public override void StateActionFixedUpdate()
    {
        base.StateActionFixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
