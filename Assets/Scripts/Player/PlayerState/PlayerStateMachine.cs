using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
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
        currentState.Exit();

        previousState = currentState;
        currentState = nextState;

        currentState.Enter();
    }
}
