using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected InputManager inputManager;
    protected Player player;

    protected Rigidbody2D rb;

    //protected float xInput;
    //protected float yInput;
    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public PlayerState(Player _player, InputManager _inputManager, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.inputManager = _inputManager;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter() 
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;

        inputManager.RegisterToDash(inputManager.Dash);
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        //xInput = Input.GetAxisRaw("Horizontal");
        //yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger() 
    {
        triggerCalled = true;
    }
}
