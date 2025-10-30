using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type1State : EnemyState
{
    protected EnemyType1 enemy;
    public Type1State(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyType1 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }
}
