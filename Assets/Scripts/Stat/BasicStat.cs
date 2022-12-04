using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Baisc Stat", menuName = "Scriptable Object/Stat")]
public class BasicStat : ScriptableObject
{
    /* 초기 데이터 (변하지 않음) */
   [SerializeField] private int hp;
   [SerializeField] private float moveSpeed;
   [SerializeField] private int armor;
   [SerializeField] private int defaultAttackDamage;

    /* 초기 스텟 프로퍼티 */
    public int Hp { get { return hp; } }   
    public float MoveSpeed { get { return moveSpeed; } }
    public int Armor { get { return armor; } }
    public int DefaultAttackDamage { get { return defaultAttackDamage; } }

}