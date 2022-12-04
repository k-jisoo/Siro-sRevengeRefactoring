using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBlade : PassiveSkill
{
    /// <summary>
    /// ��ų ����Ʈ ������Ʈ
    /// </summary>
    [SerializeField] GameObject spellBladeEffect;
    private readonly string[] spellBaldeSFX = { "Player/Passive Skill/SpellBlade_1", "Player/Passive Skill/SpellBlade_2" };

    #region ��ų �ʱ� ���� ������
    /// <summary>
    /// ��ų �⺻ ���ݷ� ���� �ۼ�Ʈ
    /// </summary>
    private int buffDamagePercent = 50;
    /// <summary>
    /// ��ų ���� ���� �� ������ ������
    /// </summary>
    private int lastBuffDamage;
    /// <summary>
    /// �÷��̾� ������ �⺻ ���ݷ�
    /// </summary>
    private int lastDefaultAttackDamage;
    /// <summary>
    /// ��ų ���ӽð�
    /// </summary>
    private float skillDuration = 5f;
    /// <summary>
    /// ��ų ���ӽð� �ڷ�ƾ
    /// </summary>
    private WaitForSeconds skillDurationSec;
    #endregion

    private Coroutine buffCoroutine;

    #region ��ų ���� ������Ƽ
    /// <summary>
    /// ���ݷ� ���� �ۼ�Ʈ ���� ������Ƽ (  set : ���� ������ buffDamagePercent �� ���� )
    /// </summary>
    public int BuffDamagePercent
    {
        set 
        {
            buffDamagePercent = value; 
            lastBuffDamage = (playerObject.DefaultAttackDamage * buffDamagePercent) / 100; // ���� �ۼ�Ʈ�� �޶������� �ٽ� ����
        }
    }
    /// <summary>
    /// ��ų ���ӽð� ������Ƽ (  set : ���� ������ ���ӽð� �ڷ�ƾ WaitForSeconds �� ���� )
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
        lastBuffDamage = (playerObject.DefaultAttackDamage * buffDamagePercent) / 100;  // ���� ������ �ʱ� ����
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
        // �÷��̾� �⺻���ݷ��� �޶��� ��� �ٽ� ���� ������ ����
        if (playerObject.DefaultAttackDamage != lastDefaultAttackDamage)
        {
            lastBuffDamage = (playerObject.DefaultAttackDamage * buffDamagePercent) / 100;
        }
        spellBladeEffect.SetActive(true);
        Managers.Sound.PlaySFXAudio(spellBaldeSFX[0]);
        playerObject.DefaultAttackDamage += lastBuffDamage;  // ���� ������ ����
        yield return skillDurationSec;
        StopSkillProcess();

    }

    /// <summary>
    /// SpellBlade ��ų �ڷ�ƾ ����
    /// </summary>
    private void StopSkillProcess()
    {
        // ��ų�� ����� ���°� �ƴϸ� ����
        if (buffCoroutine == null) return;

        Managers.Sound.PlaySFXAudio(spellBaldeSFX[1]);
        StopCoroutine(buffCoroutine);  // ��ų �ڷ�ƾ �ٷ� ����
        buffCoroutine = null;          // �ڷ�ƾ �ʱ�ȭ
        OnCoolTime();
        playerObject.DefaultAttackDamage -= lastBuffDamage;  // ���� ������ ��ü
        spellBladeEffect.SetActive(false);
    }
}


