using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierEvent : MonoBehaviour
{

    Barrier barrier;
    private readonly string barrierHitSFX = "Player/Active Skill/Barrier Hit";

    private void Awake()
    {
        barrier = GetComponentInParent<Barrier>();
    }

    private void OnCompleteEvent() // Barrier Anim 에서 이벤트 호출
    {
        barrier.BarrierAnimator.SetTrigger("OnAcitveBarrier");
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.CompareTag(Define.StringTag.Enemy.ToString()))
        {
            Managers.Sound.PlaySFXAudio(barrierHitSFX, null, 0.25f);
            target.GetComponent<Enemy>().TakeDamage(barrier.SkillDamage);
        }
    }
}

