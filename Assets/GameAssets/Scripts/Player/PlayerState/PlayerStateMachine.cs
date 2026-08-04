using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState { get; set; }
    public PlayerState previousState { get; set; }

    public void Initialize(PlayerState initState)
    {
        currentState = initState;
        previousState = currentState;
        currentState.Enter();
    }

    public void ChangeState(PlayerState nextState)
    {
        // 같은 상태 재진입 방지
        if (currentState == nextState) return;

        currentState.Exit();

        previousState = currentState;
        currentState = nextState;

        currentState.Enter();
    }
}
