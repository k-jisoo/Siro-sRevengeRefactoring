using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAid : PassiveSkill
{
    [SerializeField] GameObject firstAidEffect;
    private readonly string firstAidSFX = "Player/Passive Skill/FirstAid";

    #region 스킬 초기 스텟 데이터
    /// <summary>
    /// 스킬 최대 체력의 회복 퍼센트
    /// </summary>
    private int buffHpRegenPercent = 10;
    /// <summary>
    /// 스킬 지속시간
    /// </summary>
    private float skillDuration = 3f;
    /// <summary>
    /// 스킬 딜레이 (고정 값) = 초당 회복력을 위한 1초 시간 코루틴
    /// </summary>
    private readonly WaitForSeconds PER_SECONDS = new WaitForSeconds(1f);
    #endregion



    /// <summary>
    ///  해당 스킬 피격 시 발동 되도록 이벤트 등록
    /// </summary>
    public override void OnActive()
    {
        playerObject.HitEvent += FirstAidSkillActive;
    }

    public override void Upgrade()
    {
        skillDuration += 1;
        SkillCoolTime -= 3f;
        buffHpRegenPercent += 5;
    }

    private void FirstAidSkillActive()
    {
        if (currentSkillState == Define.CurrentSkillState.ACTIVE)
        {
            currentSkillState = Define.CurrentSkillState.COOL_TIME;
            StartCoroutine(FirstAidSkillProcess(skillDuration, (playerObject.MaxHp * buffHpRegenPercent) / 100));
        }
        else
        {
            return;
        }
    }

    private IEnumerator FirstAidSkillProcess(float buffDuration, int addHP)
    {
        firstAidEffect.SetActive(true);
        while(buffDuration > 0)
        { 
            playerObject.Hp += addHP;
            Managers.UI.UpdatePlayerHpSlider(playerObject.Hp, playerObject.MaxHp);
            Managers.Sound.PlaySFXAudio(firstAidSFX);
            buffDuration--;
            yield return PER_SECONDS;
        }

        OnCoolTime();
        firstAidEffect.SetActive(false);
    }
}
