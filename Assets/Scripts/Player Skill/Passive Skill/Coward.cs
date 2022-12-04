using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coward : PassiveSkill
{
    [SerializeField] GameObject cowardEffect;
    private readonly string cowardSFX = "Player/Passive Skill/Coward";

    #region 스킬 초기 스텟 데이터
    /// <summary>
    /// 스킬 이동속도
    /// </summary>
    private int buffSpeed = 2;  //초기 데이터 1
    /// <summary>
    /// 스킬 지속시간
    /// </summary>
    private float skillDuration = 1.5f; // 초기 데이터 1.5f 
    /// <summary>
    /// 스킬 지속시간 코루틴
    /// </summary>
    private WaitForSeconds skillDurationSec;
    #endregion

    #region 스킬 스텟 프로퍼티
    /// <summary>
    /// 스킬 지속시간 프로퍼티 (  set : 겁쟁이 지속시간 코루틴 WaitForSeconds 값 변경 )
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
    ///  해당 스킬 피격 시 발동 되도록 이벤트 등록
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
        playerObject.MoveSpeed += buffSpeed; // 속도 버프 적용
        yield return skillDurationSec;
        playerObject.MoveSpeed -= buffSpeed; // 속도 버프 해체
        cowardEffect.SetActive(false);
        OnCoolTime();
    }
}
