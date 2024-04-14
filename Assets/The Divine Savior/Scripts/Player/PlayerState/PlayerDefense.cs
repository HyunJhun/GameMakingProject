using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefense : PlayerState
{
    public PlayerDefense(Player player, Status stats, PlayerStateMachine playerStateMachine) : base(player, stats, playerStateMachine)
    { }
    //float distanceBetweenCurrentToAfter = 3f;
    public override void Enter()
    {
        player.GetPlayerAnimationManager().GetPlayerAnimator().SetTrigger("Block");
        player.GetPlayerAnimationManager().GetPlayerAnimator().SetBool("isBlock", true);
        stats.SetArmor(player.f_PlayerDefenseArmor);
    }
    public override void StateActionUpdate()
    {
        if(Input.GetButtonUp("Block"))
        {
            playerStateMachine.ChangeState(player.idleState);
            return;
        }   
    }
    public override void Exit()
    {
        player.GetPlayerAnimationManager().GetPlayerAnimator().SetBool("isBlock", false);
        stats.SetArmor(player.f_PlayerGeneralArmor);
    }

}
