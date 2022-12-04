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
    /// ����ü�� Ȱ��ȭ �� ��� ����ü ������ ����
    /// </summary>
    private void OnEnable()
    {
        knifteRigidBody.velocity = Dir * ProjectileSpeed;
        Invoke(nameof(DisableObject), 3f);                   // 3�ʵ� �ڵ� �Ҹ�
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
