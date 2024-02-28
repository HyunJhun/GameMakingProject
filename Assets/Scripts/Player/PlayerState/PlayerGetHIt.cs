using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetHIt : PlayerState
{
    public PlayerGetHIt(Player player, Status stats, PlayerStateMachine playerStateMachine) : base(player, stats, playerStateMachine)
    { }

    public override void Enter()
    {
        base.Enter();
    }
    public override void StateActionUpdate()
    {

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
