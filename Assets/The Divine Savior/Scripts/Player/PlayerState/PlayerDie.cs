using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : PlayerState
{
    public PlayerDie(Player player, Status stats, PlayerStateMachine playerStateMachine) : base(player, stats, playerStateMachine)
    { }
    public override void Enter()
    {
        MonoBehaviour.Instantiate(player.deadUiPrefab, GameObject.Find("Canvas_UI").transform);
        player.b_IsDie = true;
        stats.CancelInvoke("staminaUp");
        player.GetPlayerAnimationManager().GetPlayerAnimator().SetTrigger("isDie");
        
    }
    public override void StateActionUpdate()
    {


    }

    public override void Exit()
    {

    }

}
