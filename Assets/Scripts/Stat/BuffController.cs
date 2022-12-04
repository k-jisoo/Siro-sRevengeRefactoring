using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ӽð� ���õ� ������ ������ ����ؾ��ϴµ�...
/// </summary>
public class BuffController
{
    private Dictionary<float, WaitForSeconds> buffTimes = new Dictionary<float, WaitForSeconds>();

    private WaitForSeconds BuffDurationTime(float newBuffTime)
    {
        WaitForSeconds buffDurationTime = null;

        if (!buffTimes.TryGetValue(newBuffTime, out buffDurationTime))
        {
            buffTimes.Add(newBuffTime,  new WaitForSeconds(newBuffTime));
        }

        return buffDurationTime;
    }

    public IEnumerator BuffTimeProcess (float newBuffTime)
    {
        yield return BuffDurationTime(newBuffTime);
    }

    public float CalculationBuff(float buffPercent, float currentBuffStat)
    {
        return (buffPercent * currentBuffStat) / 100;
    }

    public int CalculationBuff(int buffPercent, int currentBuffStat)
    {
        return (buffPercent * currentBuffStat) / 100;
    }




}
