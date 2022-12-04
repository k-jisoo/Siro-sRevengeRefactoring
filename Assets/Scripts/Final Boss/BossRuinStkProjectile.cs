using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRuinStkProjectile : Projectile
{

    private Rigidbody2D RuinStkRigidBody;
    private Animator RuinStkAnimator;

    private void Awake()
    {
        RuinStkAnimator = GetComponent<Animator>();
        RuinStkRigidBody = GetComponent<Rigidbody2D>();
    }

   
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag(Define.StringTag.Player.ToString()))
        {
            target.GetComponent<Player>().TakeDamage(ProjectileDamage);
        }
    }


}
