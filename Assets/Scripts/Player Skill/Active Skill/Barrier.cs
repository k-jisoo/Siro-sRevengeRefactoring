using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : ActiveSkill
{
   [SerializeField] GameObject barrierEffectObject;
    private readonly string barrierOnSFX = "Player/Active Skill/Barrier On";

    private CircleCollider2D barrierCollider; 
    private Animator barrierAnimator;
    public Animator BarrierAnimator { get { return barrierAnimator; } }

    private Coroutine barrierCheck = null;


    #region 스킬 기본 스텟 데이터 
    /// <summary>
    /// 스킬 지속시간
    /// </summary>
    private float skillDuration = 5f;
    /// <summary>
    /// 스킬 지속시간 코루틴
    /// </summary>
    private WaitForSeconds skillDurationTimeSec;
    /// <summary>
    /// 스킬 데미지
    /// </summary>
    private int skillDamage = 10;
    /// <summary>
    /// 방벽 스킬 사이즈
    /// </summary>
    private Transform barrierSize;
    /// <summary>
    /// 방벽 공격간격 시간
    /// </summary>
    private float barrierAttackDelay = 1f;
    /// <summary>
    /// 방벽 공격간격 시간 코루틴
    /// </summary>
    private WaitForSeconds barrierAttackDelayTimeSec;
    #endregion

    #region 스킬 스텟 프로퍼티
    /// <summary>
    /// 스킬 지속시간 프로퍼티 ( set : 지속시간 코루틴 WaitForSeconds 값 변경 )
    /// </summary>
    public float SkillDuration
    {
        set
        {
            if(skillDuration != value)
            {
                skillDurationTimeSec = new WaitForSeconds(value);
            }
            skillDuration = value;
        }
    }
    /// <summary>
    /// 스킬 데미지 프로퍼티 ( get : 스킬 데미지 값, set : 스킬 데미지 값 변경 )
    /// </summary>
    public int SkillDamage 
    { 
        get { return skillDamage; }
        set { skillDamage = value; }
    } 
    /// <summary>
    /// 방벽 스킬 공격간격 시간 프로퍼티 ( set : 공견간격 시간 코루틴 WaitForSeconds 값 변경 )
    /// </summary>
    public float BarrierAttackDelay
    {
        set
        {
            if (barrierAttackDelay != value)
            {
                skillDurationTimeSec = new WaitForSeconds(value);
            }
            barrierAttackDelay = value;
        }
    }
    #endregion

    private void Start()
    {
        BarrierInit();
    }

    private void BarrierInit()
    {
        skillDurationTimeSec = new WaitForSeconds(skillDuration); // 스킬 초기 지속시간 값 설정
        barrierAttackDelayTimeSec = new WaitForSeconds(barrierAttackDelay); // 방벽 초기 공격간격 시간 값 설정

        barrierAnimator = barrierEffectObject.GetComponent<Animator>();
        barrierCollider = barrierEffectObject.GetComponent<CircleCollider2D>();
        barrierSize = barrierEffectObject.GetComponent<Transform>();
    }
    
    public override void OnActive()
    {
        if (currentSkillState == Define.CurrentSkillState.ACTIVE)
        {
            currentSkillState = Define.CurrentSkillState.COOL_TIME;
            barrierCheck = StartCoroutine(BarrierSkillProcess());
        }
        else
        {
            return;
        }
    }

    public override void Upgrade()
    {
        skillDamage += 1;
        SkillCoolTime -= 2f;
        barrierSize.localScale += new Vector3(0.5f, 0.5f, 1f);
        BarrierAttackDelay =  (barrierAttackDelay - 0.1f);
        SkillDuration = (skillDuration + 1f);
    }

    private void BarrierSkillActive()
    {
        barrierEffectObject.SetActive(true);
        barrierCollider.enabled = true;
        Managers.Sound.PlaySFXAudio(barrierOnSFX, null, 0.5f);
        StartCoroutine(BarrierHitBox());
    }

    private void BarrierSkillDisable()
    {
       if(barrierCheck != null)
       {
            StopCoroutine(barrierCheck);
       }

        StopCoroutine(BarrierHitBox());
        barrierCheck = null;
        barrierCollider.enabled = false;
        barrierEffectObject.SetActive(false);
        OnCoolTime();
    }

    private IEnumerator BarrierSkillProcess()
    {
        BarrierSkillActive();

        yield return skillDurationTimeSec;

        BarrierSkillDisable();
    }

    private IEnumerator BarrierHitBox()
    {
        while (true)
        {
            barrierCollider.enabled = true;
            yield return barrierAttackDelayTimeSec;
            barrierCollider.enabled = false;
        }
    }

}
