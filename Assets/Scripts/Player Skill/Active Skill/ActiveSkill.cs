using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveSkill : PlayerSkill
{
    /// <summary>
    /// ��ų ���� �Լ�
    /// </summary>
    public abstract void OnActive();

    /// <summary>
    /// ��ų ���׷��̵� �Լ�
    /// </summary>
    public abstract void Upgrade();
}
