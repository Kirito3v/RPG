using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, InputManager _inputManager, PlayerStateMachine _playerSM, string _animBoolName) : base(_player, _inputManager, _playerSM, _animBoolName)
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

        if (/*Input.GetKeyDown(KeyCode.L)*/ inputManager.attack.WasPressedThisFrame())
            playerSM.ChangeState(player.ATK1State);

        if (!player.isGroundDetected())
            playerSM.ChangeState(player.airState);

        if (/*Input.GetKeyDown(KeyCode.Space)*/ inputManager.jump.WasPressedThisFrame() && player.isGroundDetected())
            playerSM.ChangeState(player.jumpState);
    }
}
