using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine 
{
    public EnemyState currnentState { get; private set; }

    public void Init(EnemyState startState)
    {
        currnentState = startState;
        currnentState.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
        currnentState.Exit();
        currnentState = newState;
        currnentState.Enter();
    }
}
