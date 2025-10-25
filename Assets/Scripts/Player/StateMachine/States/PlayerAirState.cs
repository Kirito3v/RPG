using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, InputManager _inputManager, PlayerStateMachine _playerSM, string _animBoolName) : base(_player, _inputManager, _playerSM, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.isWallDetected())
            playerSM.ChangeState(player.slideState);

        if (player.isGroundDetected())
            playerSM.ChangeState(player.idleState);

        if (/*xInput*/ inputManager.GetMovingReading().x != 0)
            player.SetVelocity(player.moveSpeed * 0.8f * /*xInput*/ inputManager.GetMovingReading().x, rb.velocity.y);
    }
}
