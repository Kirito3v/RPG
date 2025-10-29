using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, InputManager _inputManager, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _inputManager, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        inputManager.RegisterToAttack(inputManager.GroundAttack);
        inputManager.RegisterToJump(inputManager.GroundJump);
        inputManager.RegisterToCounterAttack(inputManager.CounterAttack);
    }

    public override void Exit()
    {
        base.Exit();
        inputManager.UnRegisterToAttack(inputManager.GroundAttack);
        inputManager.UnRegisterToJump(inputManager.GroundJump);
        inputManager.UnRegisterToCounterAttack(inputManager.CounterAttack);
    }

    public override void Update()
    {
        base.Update();

        //if (Input.GetKeyDown(KeyCode.V) /*inputManager.counterAttack.WasPressedThisFrame()*/)
        //    stateMachine.ChangeState(player.counterAttackState);

        //if (/*Input.GetKeyDown(KeyCode.L)*/ inputManager.attack.WasPressedThisFrame())
        //    playerSM.ChangeState(player.ATK1State);

        if (!player.isGroundDetected())
            stateMachine.ChangeState(player.airState);

        //if (/*Input.GetKeyDown(KeyCode.Space)*/ inputManager.jump.WasPressedThisFrame() && player.isGroundDetected())
        //    playerSM.ChangeState(player.jumpState);
    }
}
