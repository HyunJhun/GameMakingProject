using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState
{
    protected Boss boss;
    protected Status stats;
    protected BossStateMachine bossStateMachine;
    public BossState(Boss boss,Status stats,BossStateMachine bossStateMachine)
    {
       
        this.boss = boss;
        this.stats = stats;
        this.bossStateMachine = bossStateMachine;
    }

    public virtual void StateActionUpdate() { }
    public virtual void Enter() { Debug.Log("State Enter"); }
    public virtual void Exit() { }

}
