using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HourGlass : PassiveSkill
{
    [SerializeField] GameObject hourGlassEffectObject;
    [SerializeField] VolumeProfile hourGlassProfile;
    private readonly string[] hourGlassSFX = { "Player/Passive Skill/HourGlass_1", "Player/Passive Skill/HourGlass_2" };

    private Animator hourGlassAnimator;

    #region ��ų �⺻ ���� ������
    /// <summary>
    /// ��ų ���ӽð� 
    /// </summary>
    private float skillDuration = 5f;
    /// <summary>
    /// ��ų ���ӽð� �ڷ�ƾ
    /// </summary>
    private WaitForSecondsRealtime skillDurationSec;
    #endregion

    #region ��ų ���� ������Ƽ
    /// <summary>
    /// ��ų ���ӽð� ������Ƽ ( set : �𷡽ð� ���ӽð� �ڷ�ƾ WaitForSeconds �� ���� )
    /// </summary>
    public float SkillDuration 
    { 
        set 
        {
            if (skillDuration != value)
            {
                skillDurationSec = new WaitForSecondsRealtime(value);
            }
            skillDuration = value;
        }
    } 
    #endregion


    private void Start()
    {
        HourGlassInit();
    }

    private void HourGlassInit()
    {
        hourGlassAnimator = hourGlassEffectObject.GetComponent<Animator>();
        hourGlassEffectObject.SetActive(false); 
        skillDurationSec = new WaitForSecondsRealtime(skillDuration);
    }

    public override void OnActive()
    {
        playerObject.HitEvent += HourGlassSkillActive;
    }

    public override void Upgrade()
    {
        SkillDuration = (skillDuration + 1f);
        SkillCoolTime -= 10f;
    }

    private void HourGlassSkillActive()
    {
        // �÷��̾� ���� ü���� 30���� ������� ����
        if (playerObject.Hp > 30) return;

        if (currentSkillState == Define.CurrentSkillState.ACTIVE)
        {
            currentSkillState = Define.CurrentSkillState.COOL_TIME;
            StartCoroutine(HourGlassSkillProcess());
        }
        else
        {
            return;
        }
    }

    private void HourGlassSkillDisable()
    {
        Managers.Sound.PlaySFXAudio(hourGlassSFX[1]);
        hourGlassAnimator.SetTrigger("ComeBack"); // ���ƿ��� �ð� �ִϸ��̼� ��� �� HourGlassEvent.cs ���� ���� ȿ�� �� �ð� ���� ��ü ����
        OnCoolTime();
        playerObject.PlayerController.isAttackalble = true;
    }

    private void OnSkillEffect()
    {
       Managers.Sound.PlaySFXAudio(hourGlassSFX[0]);
       Managers.SkillEffectVolume.ChagnePostProcessProfile(hourGlassProfile); // Hour Glass ��ų ����Ʈ ���μ��� ȿ�� Ȱ��ȭ
    }

    private IEnumerator HourGlassSkillProcess()
    {
        playerObject.PlayerController.isAttackalble = false;

        Time.timeScale = 0f; // �ð� ���� 
        OnSkillEffect();
        hourGlassEffectObject.SetActive(true); // Hour Glass Effect ������Ʈ Ȱ��ȭ

        yield return skillDurationSec;

        HourGlassSkillDisable();
    }


}
