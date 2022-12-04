using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{ 
    protected Player playerObject;

    protected int enemyLayer = 1 << 11 | 1 << 14;
    protected int wallLayer = 1 << 12;


    #region 스킬 레벨
    protected short skillLevel = 1;
    public short SkillLevel { get { return skillLevel; }  set { skillLevel = value; } }
    #endregion

    /// <summary>
    /// 스킬 상태
    /// </summary>
    public Define.CurrentSkillState currentSkillState;

    #region 스킬 쿨타임 변수
    /// <summary>
    /// 코루틴 WaitForSeconds 변수
    /// </summary>
    private WaitForSeconds skillCoolTimeSec; 
    /// <summary>
    /// 해당 스킬 쿨타임
    /// </summary>
   [SerializeField] private float skillCoolTime;
    /// <summary>
    /// 스킬 쿨타임 프로퍼티  ( set : 스킬 쿨타임 코루틴 WaitForSeconds 값 변경 )
    /// </summary>
    protected float SkillCoolTime                
    {
        set 
        {
            if ((skillCoolTime - value) <= 0) value = 1f;

            if (skillCoolTime != value)  // 기존 스킬 쿨타임이 변경되면 코루틴 WaitForSeconds 값도 변경
            { 
                skillCoolTimeSec = new WaitForSeconds(value);
            }
            skillCoolTime = value;
        }
        get { return skillCoolTime; }
    }
    #endregion

    public void Init(Player playerObject)
    {
        this.playerObject = playerObject;
        skillCoolTimeSec = new WaitForSeconds(skillCoolTime);  // 초기 값을 이용해 코루틴 WaitForSeconds 생성
        currentSkillState = Define.CurrentSkillState.ACTIVE;
    }

    /// <summary>
    /// 스킬 쿨타임 실행 함수
    /// </summary>
    protected void OnCoolTime()
    {
        if (currentSkillState != Define.CurrentSkillState.COOL_TIME) return;
        playerObject.OnActiveSkillEvent?.Invoke();
        StartCoroutine(SkillCoolTimeProcess());
    }

    /// <summary>
    /// 스킬 쿨타임 코루틴 함수
    /// </summary>
    /// <returns>해당 스킬 쿨타임</returns>
    private IEnumerator SkillCoolTimeProcess()
    {
        Managers.UI.CoolTimeProcess();                          //스킬을 쓸 때마다 UI 쿨타임 함수 실행.

        yield return skillCoolTimeSec;
                        
        currentSkillState = Define.CurrentSkillState.ACTIVE;   //쿨이 끝날 때도 UI 쿨타임 함수 실행
        Managers.UI.CoolTimeProcess();
    }

}
