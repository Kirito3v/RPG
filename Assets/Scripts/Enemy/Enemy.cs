using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask watIsPlayer;

    [Header("Stunned info")]
    public float stunDuration;
    public Vector2 stunDir;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImg;

    [Header("Movement info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;

    [Header("Attack info")]
    public float attackDistance;
    public float attackRadius;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;

    public EnemyStateMachine stateMachine {  get; protected set; }
    public string lastAnimBoolName {  get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currnentState.Update();
    }

    public virtual void AssignLastAnimName(string animName) => lastAnimBoolName = animName;

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    public virtual void OpenCounterAttackWindow() 
    {
        canBeStunned = true;
        counterImg.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow() 
    {
        canBeStunned = false;
        counterImg.SetActive(false);
    }

    public void AnimationTrigger() => stateMachine.currnentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPLayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, watIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }

}
