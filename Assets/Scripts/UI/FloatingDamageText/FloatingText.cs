using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    TextMeshPro damageText;

    /// <summary>
    /// ������ ��� ������Ƽ
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
