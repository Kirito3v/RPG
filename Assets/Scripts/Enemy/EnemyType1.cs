using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : Enemy
{
    #region States
    public Type1StunnedState stunnedState {  get; private set; }
    public Type1GroundState groundState {  get; protected set; }
    public Type1IdleState IdleState { get; private set; }
    public Type1MoveState MoveState { get; private set; }
    public Type1BattleState BattleState { get; private set; }
    public Type1AttackState AttackState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        
        #region States
        IdleState = new Type1IdleState(this, StateMachine, "Idle", this);
        MoveState = new Type1MoveState(this, StateMachine, "Move", this);
        BattleState = new Type1BattleState(this, StateMachine,"Move", this);
        AttackState = new Type1AttackState(this, StateMachine, "Attack", this);
        stunnedState = new Type1StunnedState(this, StateMachine, "Stunned", this);
        #endregion
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Init(IdleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            StateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }
}
