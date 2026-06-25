using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine
{
    public BossState currentState { get; set; }
    public BossState previousState { get; set; }

    public void Initialize(BossState initState)
    {
        currentState = initState;
        previousState = currentState;
        currentState.Enter();
    }

    public void ChangeState(BossState nextState)
    {
        currentState.Exit();

        previousState = currentState;
        currentState = nextState;

        currentState.Enter();
    }
}
