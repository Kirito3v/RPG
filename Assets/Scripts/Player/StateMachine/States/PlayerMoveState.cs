using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player _player, InputManager _inputManager, PlayerStateMachine _playerSM, string _animBoolName) : base(_player, _inputManager, _playerSM, _animBoolName)
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

        player.SetVelocity(/*xInput*/ inputManager.GetMovingReading().x * player.moveSpeed, rb.velocity.y);

        if (/*xInput*/ inputManager.GetMovingReading().x == 0 || player.isWallDetected())
            playerSM.ChangeState(player.idleState);
    }
}
