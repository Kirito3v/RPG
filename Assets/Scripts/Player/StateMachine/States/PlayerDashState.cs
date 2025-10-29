using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, InputManager _inputManager, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _inputManager, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0, rb.velocity.y);
        inputManager.UnRegisterToDash(inputManager.Dash);
    }

    public override void Update()
    {
        base.Update();

        if (!player.isGroundDetected() && player.isWallDetected())
            stateMachine.ChangeState(player.slideState);

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }
}
