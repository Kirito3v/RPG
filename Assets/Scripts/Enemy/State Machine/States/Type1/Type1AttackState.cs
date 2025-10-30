using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type1AttackState : Type1State
{
    public Type1AttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyType1 _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    
    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;
    }

}
