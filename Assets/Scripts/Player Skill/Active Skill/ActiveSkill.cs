using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkill : PlayerSkill
{
    /// <summary>
    /// 스킬 실행 함수
    /// </summary>
    public abstract void OnActive();

    /// <summary>
    /// 스킬 업그레이드 함수
    /// </summary>
    public abstract void Upgrade();
}
