using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprnt : PlayerState
{
    public PlayerSprnt(Player player, Status stats, PlayerStateMachine playerStateMachine) : base(player, stats, playerStateMachine)
    { }

    PlayerMoving playerMovingState;
    public override void Enter()
    {
        playerMovingState = player.movingState;
        if (player.GetPlayerStatus().getStamina() < 1)
        {
            playerStateMachine.ChangeState(player.idleState);
            return;
        }

        player.b_IsSprint = true;
        player.GetPlayerStatus().InvokeRepeating("staminaDown_Sprint", 1f, 1f);

    }
    public override void StateActionUpdate()
    {
        if(Input.GetButtonUp("Sprint"))
        {
            playerStateMachine.ChangeState(player.idleState);
            return;
        }
    }
    public override void StateActionFixedUpdate()
    {
        if (Input.GetButton("Sprint"))
        {
            playerMovingState.OnMove(player.f_PlayerSprintSpeed);
        }
    }

    public override void Exit()
    {
        player.b_IsSprint = false;
        player.GetPlayerStatus().CancelInvoke("staminaDown_Sprint");
    }
}
