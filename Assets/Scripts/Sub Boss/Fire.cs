using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Projectile
{
    private Animator Animator;
    float burnDelay = 0.5f;
    bool isBurn = true;
    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }
    IEnumerator Burning()
    {
        yield return new WaitForSeconds(burnDelay);
        Animator.SetBool("isBurn",true);
    }
    private void DisableObject()
    {
        isBurn = true;
        Animator.SetBool("isBurn", false);
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
            target.GetComponent<Player>().TakeDamage(ProjectileDamage);
        }
    }
    void BurnSound()
    {
        if (isBurn)
        {
            isBurn = false;
            Managers.Sound.PlaySFXAudio("SubBoss/불타는-효과음");
        }
       
    }
}

