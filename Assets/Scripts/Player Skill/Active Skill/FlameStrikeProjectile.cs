using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameStrikeProjectile : Projectile
{
    private CircleCollider2D flameStrikeColilder;

    private void Awake()
    {
        FlameStrikeInit();
    }

    private void FlameStrikeInit()
    {
        flameStrikeColilder = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag(TargetTag.ToString()))
        {
            target.GetComponent<Enemy>().TakeDamage(ProjectileDamage);
        }
    }




}
