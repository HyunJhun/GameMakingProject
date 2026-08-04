using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentState { get; set; }
    public EnemyState previousState { get; set; }

    public void Initialize(EnemyState initState)
    {
        currentState = initState;
        previousState = currentState;
        currentState.Enter();
    }

    public void ChangeState(EnemyState nextState)
    {
        if (currentState == nextState) return;
        currentState.Exit();

        previousState = currentState;
        currentState = nextState;

        currentState.Enter();
    }
}
