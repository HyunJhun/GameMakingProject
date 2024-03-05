using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : PlayerState
{
    public PlayerDie(Player player, Status stats, PlayerStateMachine playerStateMachine) : base(player, stats, playerStateMachine)
    { }
    public override void Enter()
    {
        player.b_IsDie = true;
        stats.CancelInvoke("StaminaIncrease");
    }
    public override void StateActionUpdate()
    {


    }

    public override void Exit()
    {

    }
}
