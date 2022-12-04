using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HourGlassEvent : MonoBehaviour
{
    private void OnCompleteEvent() // HourGlass Anim 'ComeBack'���� �̺�Ʈ Ʈ���ŷ� ȣ��
    {
        Time.timeScale = 1f;  // �ð� ���� ��ü
        OffSkillEffect();
        gameObject.SetActive(false); // ������ ��Ȱ��ȭ
    }

    private void OffSkillEffect()
    {
        Managers.SkillEffectVolume.ChagnePostProcessProfile(null); // Hour Glass ��ų ����Ʈ ���μ��� ȿ�� ��ü
    }
}
