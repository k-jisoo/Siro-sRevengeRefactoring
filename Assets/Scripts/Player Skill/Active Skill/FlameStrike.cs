using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class FlameStrike : ActiveSkill
{
    [SerializeField] GameObject firePillarObject;
    [SerializeField] VolumeProfile firePillarProfile;
    private readonly string[] flameStrikeSFX = { "Player/Active Skill/Flame Strike_1", "Player/Active Skill/Flame Strike_2" };

    #region ��ų �⺻ ���� ������
    /// <summary>
    /// ��ų ������
    /// </summary>
    private int skillDamgae = 60;
    /// <summary>
    /// �ұ�� ��ų ��ȯ ����
    /// </summary>
    private int skillProjectileCount = 1; // �ʱ� �� 1��
    /// <summary>
    /// �ұ�� ��ȯ ���ݽð�
    /// </summary>
    private readonly float skillAttackDelay = 0.25f; // �ʱ� �� 0��
    /// <summary>
    /// �ұ�� ��ȯ���� �ð� �ڷ�ƾ  
    /// </summary>
    private WaitForSeconds skillAttackDelayTimeSec;
    #endregion

    private void Start()
    {
        FlameStrikeInit();
    }

    private void FlameStrikeInit()
    {
        skillAttackDelayTimeSec = new WaitForSeconds(skillAttackDelay);
    }

    public override void OnActive()
    {
        if (currentSkillState == Define.CurrentSkillState.ACTIVE)
        {
            currentSkillState = Define.CurrentSkillState.COOL_TIME;
            FlameStrikeSkillAttack();
        }
        else
        {
            return;
        }
    }

    public override void Upgrade()
    {
        skillDamgae += 20;
        SkillCoolTime -= 2;
        skillProjectileCount += 2;
    }

    private void FlameStrikeSkillAttack()
    {
        StartCoroutine(FlameStrikeSkillProcess());
    }

    IEnumerator FlameStrikeSkillProcess()
    {
        OnSkillEffect();

        for (int i = 0; i < skillProjectileCount; i++)
        { 
            GameObject projectile = MemoryPoolManager.GetInstance().OutputGameObject(firePillarObject, 
                                                                                     "Player_Skill/"+firePillarObject.name,
                                                                                     new Vector2(transform.position.x + Random.Range(-4f, 4f), transform.position.y + Random.Range(-4f, 4f)),
                                                                                     Quaternion.identity); 
            projectile.GetComponent<Projectile>().ProjectileInit(Define.StringTag.Enemy, Vector2.zero, skillDamgae);
            projectile.SetActive(true);
            Managers.Sound.PlaySFXAudio(flameStrikeSFX[0]);
            Managers.Sound.PlayDelaySFXAudio(flameStrikeSFX[1], 1f, null, 1f);
            yield return skillAttackDelayTimeSec; // �ұ�� ��ȯ���� �ð�
        }

       Invoke(nameof(OffSkillEffect), 2f); //  2�ʵ� ����Ʈ ��Ȱ��ȭ ;
        OnCoolTime();
    }

    private void OnSkillEffect()
    {
        Managers.SkillEffectVolume.ChagnePostProcessProfile(firePillarProfile);
    }

    private void OffSkillEffect()
    {
        Managers.SkillEffectVolume.ChagnePostProcessProfile(null);
    }

}
