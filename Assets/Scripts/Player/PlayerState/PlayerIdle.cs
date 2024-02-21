using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    public PlayerIdle(Player player, Status stats, PlayerStateMachine playerStateMachine) : base(player, stats, playerStateMachine)
    { }

    public override void Enter()
    {
        base.Enter();
    }
    public override void StateActionUpdate()
    {
        if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {

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
