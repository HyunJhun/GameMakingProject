using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloating : PlayerState
{
    public PlayerFloating(Player player, Status stats, PlayerStateMachine playerStateMachine) : base(player, stats, playerStateMachine)
    { }
    public override void Enter()
    {
        player.b_IsFloating = true;
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
    }
}
