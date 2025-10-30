using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type1BattleState : Type1State
{
    private Transform player;
    private int moveDir;

    public Type1BattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyType1 _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
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

        if (enemy.IsPLayerDetected()) 
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPLayerDetected().distance < enemy.attackDistance - enemy.attackRadius)
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
                stateMachine.ChangeState(enemy.idleState);
        }

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public bool CanAttack() 
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown) 
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }

}
