using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : BossState
{
    public BossAttack(Boss boss,Status stats,BossStateMachine bossStateMachine) : base(boss,stats,bossStateMachine)
    {
    }

    private float bossMoveSpeedToAttack = 2f;
    private Vector3 attackDirection;
    private float timer = 0f;

    public override void Enter()
    {

    }
    public override void Exit()
    {
        Debug.Log("DetectExit");
    }
    public override void StateActionUpdate()
    {
        attackDirection = boss.transform.position - boss.player.transform.position;

        // 공격을 하는 동시에 상대방을 향해 전진하며 가까이 다가가 상대가 움직였을 때를 방지한다. 
    }

    public override void StateActionFixedUpdate()
    {
        boss.GetComponent<Rigidbody>().AddForce(attackDirection * bossMoveSpeedToAttack * Time.fixedDeltaTime);
    }


}
