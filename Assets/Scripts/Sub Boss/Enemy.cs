using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : LivingEntity, IBasicMovement
{
    protected Player playerTarget;

    private Rigidbody2D enemyRigidbody;
    private Animator enemyAnimator;
    private Collider2D enemyCollider;
    private SpriteRenderer spriteRenderer;
    public Rigidbody2D EnemyRigidbody { get { return enemyRigidbody; } }
    public Animator EnemyAnimator { get { return enemyAnimator; } }
    public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } }
    public Collider2D EnemyCollider { get { return enemyCollider; } }

    protected void Start()
    {
        BasicStatInit();
    }

    public void EnemyInit(Player playerTarget)
    {
        this.playerTarget = playerTarget;
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<Collider2D>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
    }

    public void DefaultAttack()
    {
        playerTarget.TakeDamage(DefaultAttackDamage);
    }

    protected override void OnDead() { }

    public void Move()
    {
        Vector2 dir = (playerTarget.transform.position - transform.position).normalized;
        enemyRigidbody.velocity = MoveSpeed * dir;
    }

}

