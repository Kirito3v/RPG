using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currnentState { get; private set; }

    public void Init(PlayerState startState) 
    {
        currnentState = startState;
        currnentState.Enter();
    }

    public void ChangeState(PlayerState newState) 
    {
        currnentState.Exit();
        currnentState = newState;
        currnentState.Enter();
    }
}
