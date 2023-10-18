using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine
{
    public BossState currentState { get; set; }

    public void Initialize(BossState initState)
    {
        currentState = initState;
        currentState.Enter();
    }

    public void ChangeState(BossState nextState)
    {
        currentState.Exit();

        currentState = nextState;

        currentState.Enter();
    }
}
