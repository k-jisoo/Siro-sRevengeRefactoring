using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    TextMeshPro damageText;

    /// <summary>
    /// 데미지 출력 프로퍼티
    /// </summary>
    public string DamageText
    {
        set { damageText.text = value; }
    }

    private void Awake()
    {
        damageText = GetComponentInChildren<TextMeshPro>();
    }

    private void OnDisable()
    {
        MemoryPoolManager.GetInstance().InputGameObject(this.gameObject); ;
    }
}
