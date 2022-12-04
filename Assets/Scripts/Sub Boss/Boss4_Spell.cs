using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4_Spell : Projectile
{
    private void DisableObject()
    {
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
    void sound()
    {
        Managers.Sound.PlaySFXAudio("SubBoss/Boss4_Spell_SFX");
    }
}
