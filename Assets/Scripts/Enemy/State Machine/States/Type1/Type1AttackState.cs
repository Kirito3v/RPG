using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type1AttackState : EnemyState
{
    private EnemyType1 enemy;
    public Type1AttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyType1 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
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
            stateMachine.ChangeState(enemy.BattleState);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;
    }

}
