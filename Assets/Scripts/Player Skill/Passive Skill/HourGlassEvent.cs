using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourGlassEvent : MonoBehaviour
{
    private void OnCompleteEvent() // HourGlass Anim 'ComeBack'에서 이벤트 트리거로 호출
    {
        Time.timeScale = 1f;  // 시간 정지 해체
        OffSkillEffect();
        gameObject.SetActive(false); // 스스로 비활성화
    }

    private void OffSkillEffect()
    {
        Managers.SkillEffectVolume.ChagnePostProcessProfile(null); // Hour Glass 스킬 포스트 프로세싱 효과 해체
    }
}
