using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAid : PassiveSkill
{
    [SerializeField] GameObject firstAidEffect;
    private readonly string firstAidSFX = "Player/Passive Skill/FirstAid";

    #region ��ų �ʱ� ���� ������
    /// <summary>
    /// ��ų �ִ� ü���� ȸ�� �ۼ�Ʈ
    /// </summary>
    private int buffHpRegenPercent = 10;
    /// <summary>
    /// ��ų ���ӽð�
    /// </summary>
    private float skillDuration = 3f;
    /// <summary>
    /// ��ų ������ (���� ��) = �ʴ� ȸ������ ���� 1�� �ð� �ڷ�ƾ
    /// </summary>
    private readonly WaitForSeconds PER_SECONDS = new WaitForSeconds(1f);
    #endregion



    /// <summary>
    ///  �ش� ��ų �ǰ� �� �ߵ� �ǵ��� �̺�Ʈ ���
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
