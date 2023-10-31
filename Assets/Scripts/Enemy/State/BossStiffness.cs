using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStiffness : BossState
{
    public BossStiffness(Boss boss, Status stats, BossStateMachine bossStateMachine) : base(boss, stats, bossStateMachine)
    {
    }


    private float timer = 0f;
    public override void Enter()
    {
        Debug.Log("StiffnesseEnter");
        timer = 0f;
    }
    public override void Exit()
    {
        Debug.Log("StiffnessExit");
    }
    public override void StateActionUpdate()
    {
        if (timer < 3f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            bossStateMachine.ChangeState(boss.chaseState);
        }

        
    }

    public override void StateActionFixedUpdate()
    {

    }
}
