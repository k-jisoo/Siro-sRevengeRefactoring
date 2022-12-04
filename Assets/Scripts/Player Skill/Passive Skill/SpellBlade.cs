using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBlade : PassiveSkill
{
    /// <summary>
    /// 스킬 이펙트 오브젝트
    /// </summary>
    [SerializeField] GameObject spellBladeEffect;
    private readonly string[] spellBaldeSFX = { "Player/Passive Skill/SpellBlade_1", "Player/Passive Skill/SpellBlade_2" };

    #region 스킬 초기 스텟 데이터
    /// <summary>
    /// 스킬 기본 공격력 버프 퍼센트
    /// </summary>
    private int buffDamagePercent = 50;
    /// <summary>
    /// 스킬 버프 받은 후 마지막 데미지
    /// </summary>
    private int lastBuffDamage;
    /// <summary>
    /// 플레이어 마지막 기본 공격력
    /// </summary>
    private int lastDefaultAttackDamage;
    /// <summary>
    /// 스킬 지속시간
    /// </summary>
    private float skillDuration = 5f;
    /// <summary>
    /// 스킬 지속시간 코루틴
    /// </summary>
    private WaitForSeconds skillDurationSec;
    #endregion

    private Coroutine buffCoroutine;

    #region 스킬 스텟 프로퍼티
    /// <summary>
    /// 공격력 버프 퍼센트 증가 프로퍼티 (  set : 버프 데미지 buffDamagePercent 값 변경 )
    /// </summary>
    public int BuffDamagePercent
    {
        set 
        {
            buffDamagePercent = value; 
            lastBuffDamage = (playerObject.DefaultAttackDamage * buffDamagePercent) / 100; // 버프 퍼센트가 달라졌으니 다시 적용
        }
    }
    /// <summary>
    /// 스킬 지속시간 프로퍼티 (  set : 버프 데미지 지속시간 코루틴 WaitForSeconds 값 변경 )
    /// </summary>
    public float SkillDuration
    {
        set
        {
            if (skillDuration != value)
            {
                skillDurationSec = new WaitForSeconds(value);
            }
            skillDuration = value;
        }
    }
    #endregion

    private void Start()
    {
        SpellBladeInit();
    }

    private void SpellBladeInit()
    {
        skillDurationSec = new WaitForSeconds(skillDuration);
        lastDefaultAttackDamage = playerObject.DefaultAttackDamage;
        lastBuffDamage = (playerObject.DefaultAttackDamage * buffDamagePercent) / 100;  // 버프 데미지 초기 적용
    }

    public override void OnActive()
    {
        playerObject.OnActiveSkillEvent += SpellBladeActive;
        playerObject.DisableBuffEvent += StopSkillProcess;
    }

    public override void Upgrade()
    {
        BuffDamagePercent = 50;
        SkillCoolTime -= 2f;
        SkillDuration = (skillDuration + 2f);
    }

    private void SpellBladeActive()
    {
        if (currentSkillState == Define.CurrentSkillState.ACTIVE)
        {
            currentSkillState = Define.CurrentSkillState.COOL_TIME;
            buffCoroutine = StartCoroutine(SpellBladeSkillProcess());
        }
        else
        {
            return;
        }
    }

    private IEnumerator SpellBladeSkillProcess()
    {
        // 플레이어 기본공격력이 달라진 경우 다시 버프 데미지 재계산
        if (playerObject.DefaultAttackDamage != lastDefaultAttackDamage)
        {
            lastBuffDamage = (playerObject.DefaultAttackDamage * buffDamagePercent) / 100;
        }
        spellBladeEffect.SetActive(true);
        Managers.Sound.PlaySFXAudio(spellBaldeSFX[0]);
        playerObject.DefaultAttackDamage += lastBuffDamage;  // 버프 데미지 적용
        yield return skillDurationSec;
        StopSkillProcess();

    }

    /// <summary>
    /// SpellBlade 스킬 코루틴 종료
    /// </summary>
    private void StopSkillProcess()
    {
        // 스킬이 적용된 상태가 아니면 무시
        if (buffCoroutine == null) return;

        Managers.Sound.PlaySFXAudio(spellBaldeSFX[1]);
        StopCoroutine(buffCoroutine);  // 스킬 코루틴 바로 종료
        buffCoroutine = null;          // 코루틴 초기화
        OnCoolTime();
        playerObject.DefaultAttackDamage -= lastBuffDamage;  // 버프 데미지 해체
        spellBladeEffect.SetActive(false);
    }
}


