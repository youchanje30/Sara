using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState curEnemyState { get; set; }
    private EnemyState prevEnemyState { get; set; }

    public void Init(EnemyState startState)
    {
        curEnemyState = startState;
        prevEnemyState = startState;
        curEnemyState.EnterState();
    }

    public void ChangeState(EnemyState newState)
    {
        curEnemyState.ExitState();
        prevEnemyState = curEnemyState;
        curEnemyState = newState;
        curEnemyState.EnterState();
    }

    public void RevertPrevState()
    {
        ChangeState(prevEnemyState);
    }

}
