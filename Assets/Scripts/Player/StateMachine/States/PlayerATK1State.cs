using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerATK1State : PlayerState
{
    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 2;

    public PlayerATK1State(Player _player, InputManager _inputManager, PlayerStateMachine _playerSM, string _animBoolName) : base(_player, _inputManager, _playerSM, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);

        float atkDir = player.facingDir;

        if (/*xInput*/ inputManager.GetMovingReading().x != 0)
            atkDir = /*xInput*/ inputManager.GetMovingReading().x;

        player.SetVelocity(player.attackMovement[comboCounter].x * atkDir, player.attackMovement[comboCounter].y);

        stateTimer = 0.05f;
    }

    public override void Exit()
    {
        base.Exit();

        player.BusyFor(0.1f).Forget();

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.ZeroVelocity();

        if (triggerCalled)
            playerSM.ChangeState(player.idleState);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
