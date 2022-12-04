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


    #region ��ų �⺻ ���� ������ 
    /// <summary>
    /// ��ų ���ӽð�
    /// </summary>
    private float skillDuration = 5f;
    /// <summary>
    /// ��ų ���ӽð� �ڷ�ƾ
    /// </summary>
    private WaitForSeconds skillDurationTimeSec;
    /// <summary>
    /// ��ų ������
    /// </summary>
    private int skillDamage = 10;
    /// <summary>
    /// �溮 ��ų ������
    /// </summary>
    private Transform barrierSize;
    /// <summary>
    /// �溮 ���ݰ��� �ð�
    /// </summary>
    private float barrierAttackDelay = 1f;
    /// <summary>
    /// �溮 ���ݰ��� �ð� �ڷ�ƾ
    /// </summary>
    private WaitForSeconds barrierAttackDelayTimeSec;
    #endregion

    #region ��ų ���� ������Ƽ
    /// <summary>
    /// ��ų ���ӽð� ������Ƽ ( set : ���ӽð� �ڷ�ƾ WaitForSeconds �� ���� )
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
    /// ��ų ������ ������Ƽ ( get : ��ų ������ ��, set : ��ų ������ �� ���� )
    /// </summary>
    public int SkillDamage 
    { 
        get { return skillDamage; }
        set { skillDamage = value; }
    } 
    /// <summary>
    /// �溮 ��ų ���ݰ��� �ð� ������Ƽ ( set : ���߰��� �ð� �ڷ�ƾ WaitForSeconds �� ���� )
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
        skillDurationTimeSec = new WaitForSeconds(skillDuration); // ��ų �ʱ� ���ӽð� �� ����
        barrierAttackDelayTimeSec = new WaitForSeconds(barrierAttackDelay); // �溮 �ʱ� ���ݰ��� �ð� �� ����

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
