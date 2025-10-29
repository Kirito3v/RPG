using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player _player, InputManager _inputManager, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _inputManager, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (/*xInput*/ inputManager.GetMovingReading().x == player.facingDir && player.isWallDetected())
            return;

        if (/*xInput*/ inputManager.GetMovingReading().x != 0 && !player.isBusy)
            stateMachine.ChangeState(player.moveState);
    }
}
