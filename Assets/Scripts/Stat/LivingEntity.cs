using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �⺻ ���ݰ� ���ó���� �����ϴ� Ŭ����
/// </summary>
public abstract class LivingEntity : MonoBehaviour
{
    [SerializeField] BasicStat basicStat; // �⺻ ����

    /* ���� ���� ������ */
    private int hp;
    private int maxHp;
    private float moveSpeed;
    private int armor;
    private int defaultAttackDamage;

    /// <summary>
    /// ����ü�� ������Ƽ  get ( hp ), set ( hp �� ���� )
    /// </summary>
    public int Hp 
    { 
        get { return hp; } 
        set 
        {
            if ((value + hp) >= maxHp) hp = maxHp;
            else hp = value;
        } 
    }

    /// <summary>
    /// �ִ�ü�� ������Ƽ  get ( maxHp ), set ( maxHp �� ���� )
    /// </summary>
    public int MaxHp
    {
        get { return maxHp; }
        set
        {
            maxHp = value;
        }
    }

    /// <summary>
    /// �̵��ӵ� ������Ƽ  get ( moveSpeed ), set (  moveSpeed �� ����)
    /// </summary>
    public float MoveSpeed 
    {
        get { return moveSpeed; }
        set
        {
            moveSpeed = value;
        }
    }
    public float Armor { get; set; }
    public int DefaultAttackDamage 
    {
        get { return defaultAttackDamage; }
        set
        {
            defaultAttackDamage = value;
        }
    }

    /// <summary>
    /// �⺻ ������ BasicStat Ŭ������ ���� ���� �޾� �ʱ�ȭ
    /// (�ִ�ü��, �̵��ӵ�, ����, �⺻ ���ݷ�)
    /// </summary>
    protected void BasicStatInit()
    {
        hp = basicStat.Hp;
        maxHp = hp;
        moveSpeed = basicStat.MoveSpeed;
        armor = basicStat.Armor;
        defaultAttackDamage = basicStat.DefaultAttackDamage;
    }

    /// <summary>
    /// ( �߻�ȭ = �����ʼ� ) ���ó�� �Լ�
    /// </summary>
    protected abstract void OnDead();

    /// <summary>
    /// ü�� ���� �Լ� (���� ü�� = ���� ������ - ����)
    /// </summary>
    /// <param name="newDamage">���� ������</param>
    public virtual void TakeDamage(int newDamage)
    {
        // �����ϰ� ���ݷ� - �������� ���

        if (newDamage <= armor) hp -= 1;
        else hp -= (newDamage - armor);  

        if (hp <= 0)
        {
            OnDead();
        }
    }
}
