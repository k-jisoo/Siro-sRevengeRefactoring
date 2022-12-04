using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeProjectile : Projectile
{
    private Rigidbody2D knifteRigidBody;

    private void Awake()
    {
        KnifeProjectileInit();
    }

    /// <summary>
    /// 투사체가 활성화 될 경우 투사체 방향을 설정
    /// </summary>
    private void OnEnable()
    {
        knifteRigidBody.velocity = Dir * ProjectileSpeed;
        Invoke(nameof(DisableObject), 3f);                   // 3초뒤 자동 소멸
    }

    private void KnifeProjectileInit()
    {
        knifteRigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag(TargetTag.ToString()))
        {
            target.GetComponent<Enemy>().TakeDamage(ProjectileDamage);
            DisableObject();
        }
    }
}
