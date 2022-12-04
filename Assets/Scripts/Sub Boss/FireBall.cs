using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Projectile
{
    private Animator Animator;
    private Rigidbody2D Rigidbody;
    float BombDelay = 1f;
    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }
    IEnumerator Bomb()
    {
        yield return new WaitForSeconds(BombDelay);
        Animator.SetBool("isBomb", true);
    }
    private void OnEnable()
    {
        Rigidbody.velocity = Dir * ProjectileSpeed;

    }
    private new void DisableObject()
    {
        Animator.SetBool("isBomb", false);
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        MemoryPoolManager.GetInstance().InputGameObject(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag(TargetTag.ToString()))
        {
            Rigidbody.velocity =Vector2.zero;
            Animator.SetBool("isBomb", true);
            target.GetComponent<Player>().TakeDamage(ProjectileDamage);
        }
    }
    
}
