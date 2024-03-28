using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy
{
    public EnemyStateMachine archerStateMachine { get; set; }
    public Animator archerAnimator { get; set; }
    private void Start()
    {
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    private void onInitialize()
    {
        archerStateMachine = new EnemyStateMachine();

        archerAnimator = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        if (GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            transform.position += GetAnimator().deltaPosition + transform.forward * f_attackMoveSpeed;
        }
    }
}
