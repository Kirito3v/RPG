using System.ComponentModel;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Player : Entity
{
    private InputManager inputManager;
    public bool isBusy { get; private set; }

    [Header("Attack info")]
    public Vector2[] attackMovement;

    [Header("Movement info")]
    public float moveSpeed;
    public float jumpForce;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }
    [SerializeField] private float dashCooldown;
    private float dashCooldownTime;

    #region stateMachineStates
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState slideState  { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerATK1State ATK1State { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        #region States
        stateMachine = new PlayerStateMachine();
        inputManager = GetComponent<InputManager>();

        idleState = new PlayerIdleState(this, inputManager, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, inputManager, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, inputManager, stateMachine, "Jump");
        airState = new PlayerAirState(this, inputManager, stateMachine, "Jump");
        dashState = new PlayerDashState(this, inputManager, stateMachine, "Dash");
        slideState = new PlayerWallSlideState(this, inputManager, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, inputManager, stateMachine, "Jump");
        ATK1State = new PlayerATK1State(this, inputManager, stateMachine, "Attack");
        #endregion
    }


    protected override void Start()
    {
        base.Start();

        stateMachine.Init(idleState);
    }

    protected override void Update()
    {
        base.Update();
        
        stateMachine.currnentState.Update();

        Dash();
    }

    public async UniTaskVoid BusyFor(float sec)
    {
        isBusy = true;

        await UniTask.Delay(TimeSpan.FromSeconds(sec));

        isBusy = false;
    }

    void Dash() 
    {
        if (isWallDetected())
            return;

        dashCooldownTime -= Time.deltaTime;

        if (/*Input.GetKeyDown(KeyCode.LeftShift)*/ inputManager.dash.IsPressed() && dashCooldownTime < 0)
        {
            dashCooldownTime = dashCooldown;
            
            dashDir = /*Input.GetAxisRaw("Horizontal")*/ inputManager.GetMovingReading().x;

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }

    public void AnimationTrigger() => stateMachine.currnentState.AnimationFinishTrigger();
}
