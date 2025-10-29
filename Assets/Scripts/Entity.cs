using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    public EntityFX fx { get; private set; }
    #endregion

    public int facingDir = 1;
    protected bool facingRight = true;

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDir;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;
    public bool isBusy { get; private set; }


    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask watIsGround;
    [Space]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    protected virtual void Awake() 
    {

    }

    protected virtual void Start()
    {
        fx = GetComponent<EntityFX>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {
        
    }

    public async UniTaskVoid BusyFor(float sec)
    {
        isBusy = true;

        await UniTask.Delay(TimeSpan.FromSeconds(sec));

        isBusy = false;
    }

    #region Damage
    public virtual void Damage() 
    {
        fx.FlashFX().Forget();
        HitKnockback().Forget();
        Debug.Log(gameObject.name + " AAAh");
    }

    protected virtual async UniTask HitKnockback() 
    {
        isKnocked = true;

        rb.velocity = new Vector2(knockbackDir.x * -facingDir, knockbackDir.y);

        await UniTask.Delay(TimeSpan.FromSeconds(knockbackDuration));

        isKnocked = false;
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    protected virtual void FlipCtrl(float x)
    {
        if (x > 0 && !facingRight)
            Flip();
        else if (x < 0 && facingRight)
            Flip();
    }

    #endregion

    #region Velocity
    public virtual void SetVelocity(float x, float y)
    {
        if (isKnocked)
            return;

        rb.velocity = new Vector2(x, y);
        FlipCtrl(x);
    }

    public virtual void SetZeroVelocity()
    {
        if (isKnocked)
            return;

        rb.velocity = new Vector2(0, 0);
    }
    #endregion

    #region Collision
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    public virtual bool isGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, watIsGround);

    public virtual bool isWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance * facingDir, watIsGround);
    #endregion
}
