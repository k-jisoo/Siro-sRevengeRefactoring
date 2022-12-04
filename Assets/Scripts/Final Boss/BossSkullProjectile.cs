using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkullProjectile : Projectile
{

    private Rigidbody2D skullRigidBody;
    private Animator skullAnimator;

    private void Awake()
    {
        skullAnimator = GetComponent<Animator>();
        skullRigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        skullRigidBody.velocity = Dir.normalized * ProjectileSpeed;
        Invoke(nameof(DisableObject),3.0f);
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.CompareTag(Define.StringTag.Player.ToString()))
        {
            target.GetComponent<Player>().TakeDamage(ProjectileDamage);
            skullAnimator.SetTrigger("DestroySkull");
            skullRigidBody.velocity = Vector2.zero;
        }
    }


}
