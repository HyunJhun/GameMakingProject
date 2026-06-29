using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    public PlayerIdle(Player player, Status stats, PlayerStateMachine playerStateMachine) : base(player, stats, playerStateMachine)
    { }

    public override void Enter()
    {
    }
    public override void StateActionUpdate()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            playerStateMachine.ChangeState(player.movingState);
            return;
        }
        else if (Input.GetButtonDown("Attack"))
        {
            playerStateMachine.ChangeState(player.offenseState);
            return;
        }
        else if (Input.GetButtonDown("Block"))
        {
            playerStateMachine.ChangeState(player.defenseState);
            return; 
        }
        else if (Input.GetButtonDown("Dodge"))
        {
            playerStateMachine.ChangeState(player.dodgeState);
            return;
        }
        else if (player.GetKeyInputManager().CheckSkillKeyInput())
        {
            playerStateMachine.ChangeState(player.spellCastingState);
            return;
        }
    }
    public override void Exit()
    {
    }
}
