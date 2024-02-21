using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    protected Player player;
    protected Status stats;
    protected PlayerStateMachine playerStateMachine;
    public PlayerState(Player player,Status stats,PlayerStateMachine playerStateMachine)
    {

        this.player = player;
        this.stats = stats;
        this.playerStateMachine = playerStateMachine;
    }

    public virtual void StateActionUpdate() { }

    public virtual void StateActionFixedUpdate() { }
    public virtual void Enter() { Debug.Log("State Enter"); }
    public virtual void Exit() { }
}
