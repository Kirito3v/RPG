using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, InputManager _inputManager, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _inputManager, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        inputManager.RegisterToJump(inputManager.WallSlideJump);
    }

    public override void Exit()
    {
        base.Exit();
        inputManager.UnRegisterToJump(inputManager.WallSlideJump);
    }

    public override void Update()
    {
        base.Update();

        //if (/*Input.GetKeyDown(KeyCode.Space)*/ inputManager.jump.WasPressedThisFrame())
        //{
        //    playerSM.ChangeState(player.wallJumpState);
        //    return;
        //}

        if (/*xInput*/ inputManager.GetMovingReading().x != 0 && player.facingDir != /*xInput*/ inputManager.GetMovingReading().x)
            stateMachine.ChangeState(player.idleState);

        if (/*yInput*/ inputManager.GetMovingReading().y < 0)
            rb.velocity = new Vector2(0, rb.velocity.y);
        else    
            rb.velocity = new Vector2(0, rb.velocity.y * 0.7f);


        if (player.isGroundDetected() || !player.isWallDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
