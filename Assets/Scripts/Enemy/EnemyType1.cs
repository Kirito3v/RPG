using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : Enemy
{
    #region States
    public Type1StunnedState stunnedState {  get; private set; }
    public Type1GroundState groundState {  get; protected set; }
    public Type1IdleState idleState { get; private set; }
    public Type1MoveState moveState { get; private set; }
    public Type1BattleState battleState { get; private set; }
    public Type1AttackState attackState { get; private set; }
    public Type1DeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        
        #region States
        idleState = new Type1IdleState(this, stateMachine, "Idle", this);
        moveState = new Type1MoveState(this, stateMachine, "Move", this);
        battleState = new Type1BattleState(this, stateMachine,"Move", this);
        attackState = new Type1AttackState(this, stateMachine, "Attack", this);
        stunnedState = new Type1StunnedState(this, stateMachine, "Stunned", this);
        deadState = new Type1DeadState(this, stateMachine, "Die", this);
        #endregion
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Init(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }
}
