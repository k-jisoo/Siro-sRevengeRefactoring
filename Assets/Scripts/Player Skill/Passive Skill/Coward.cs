using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coward : PassiveSkill
{
    [SerializeField] GameObject cowardEffect;
    private readonly string cowardSFX = "Player/Passive Skill/Coward";

    #region ��ų �ʱ� ���� ������
    /// <summary>
    /// ��ų �̵��ӵ�
    /// </summary>
    private int buffSpeed = 2;  //�ʱ� ������ 1
    /// <summary>
    /// ��ų ���ӽð�
    /// </summary>
    private float skillDuration = 1.5f; // �ʱ� ������ 1.5f 
    /// <summary>
    /// ��ų ���ӽð� �ڷ�ƾ
    /// </summary>
    private WaitForSeconds skillDurationSec;
    #endregion

    #region ��ų ���� ������Ƽ
    /// <summary>
    /// ��ų ���ӽð� ������Ƽ (  set : ������ ���ӽð� �ڷ�ƾ WaitForSeconds �� ���� )
    /// </summary>
    public float SkillDuration 
    { 
        set 
        { 
            if(skillDuration != value)
            {
                skillDurationSec = new WaitForSeconds(value);
            }
            skillDuration = value; 
        }     
    }
    #endregion

    private void Start()
    {
        CowardInit();
    }

    private void CowardInit()
    {
        skillDurationSec = new WaitForSeconds(skillDuration);
    }

    /// <summary>
    ///  �ش� ��ų �ǰ� �� �ߵ� �ǵ��� �̺�Ʈ ���
    /// </summary>
    public override void OnActive()
    {
        playerObject.HitEvent += CowardSkillActive;
    }

    public override void Upgrade()
    {
        SkillDuration = (skillDuration + 1f);
        SkillCoolTime -= 2f;
        buffSpeed += 2;
    }

    private void CowardSkillActive()
    {
        if (currentSkillState == Define.CurrentSkillState.ACTIVE)
        {
            currentSkillState = Define.CurrentSkillState.COOL_TIME;
            StartCoroutine(CowardSkillProcess());
        }
        else
        {
            return;
        }
    }

    private IEnumerator CowardSkillProcess()
    {
        cowardEffect.SetActive(true);
        Managers.Sound.PlaySFXAudio(cowardSFX);
        playerObject.MoveSpeed += buffSpeed; // �ӵ� ���� ����
        yield return skillDurationSec;
        playerObject.MoveSpeed -= buffSpeed; // �ӵ� ���� ��ü
        cowardEffect.SetActive(false);
        OnCoolTime();
    }
}
