using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class FlameStrike : ActiveSkill
{
    [SerializeField] GameObject firePillarObject;
    [SerializeField] VolumeProfile firePillarProfile;
    private readonly string[] flameStrikeSFX = { "Player/Active Skill/Flame Strike_1", "Player/Active Skill/Flame Strike_2" };

    #region 스킬 기본 스텟 데이터
    /// <summary>
    /// 스킬 데미지
    /// </summary>
    private int skillDamgae = 60;
    /// <summary>
    /// 불기둥 스킬 소환 개수
    /// </summary>
    private int skillProjectileCount = 1; // 초기 값 1개
    /// <summary>
    /// 불기둥 소환 간격시간
    /// </summary>
    private readonly float skillAttackDelay = 0.25f; // 초기 값 0초
    /// <summary>
    /// 불기둥 소환간격 시간 코루틴  
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
            yield return skillAttackDelayTimeSec; // 불기둥 소환간격 시간
        }

       Invoke(nameof(OffSkillEffect), 2f); //  2초뒤 이펙트 비활성화 ;
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
