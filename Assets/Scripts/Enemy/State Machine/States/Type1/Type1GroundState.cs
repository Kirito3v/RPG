using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type1GroundState : Type1State
{
    protected Transform player;

    public Type1GroundState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyType1 _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.Instance.player.transform;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPLayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 2)
            stateMachine.ChangeState(enemy.battleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
