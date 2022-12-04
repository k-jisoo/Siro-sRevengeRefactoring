using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Baisc Stat", menuName = "Scriptable Object/Stat")]
public class BasicStat : ScriptableObject
{
    /* �ʱ� ������ (������ ����) */
   [SerializeField] private int hp;
   [SerializeField] private float moveSpeed;
   [SerializeField] private int armor;
   [SerializeField] private int defaultAttackDamage;

    /* �ʱ� ���� ������Ƽ */
    public int Hp { get { return hp; } }   
    public float MoveSpeed { get { return moveSpeed; } }
    public int Armor { get { return armor; } }
    public int DefaultAttackDamage { get { return defaultAttackDamage; } }

}