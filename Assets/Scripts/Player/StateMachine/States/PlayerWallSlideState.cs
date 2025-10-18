using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _playerSM, string _animBoolName) : base(_player, _playerSM, _animBoolName)
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

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            playerSM.ChangeState(player.wallJumpState);
            return;
        }

        if (yInput < 0)
            rb.velocity = new Vector2(0, rb.velocity.y);
        else    
            rb.velocity = new Vector2(0, rb.velocity.y * 0.7f);

        if (xInput != 0 && player.facingDir != xInput)
            playerSM.ChangeState(player.idleState);

        if (player.isGroundDetected() || !player.isWallDetected())
            playerSM.ChangeState(player.idleState);
    }
}
