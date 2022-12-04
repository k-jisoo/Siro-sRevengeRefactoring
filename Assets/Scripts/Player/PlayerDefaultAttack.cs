using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefaultAttack : MonoBehaviour
{
    #region �÷��̾� ���� ����
    private Player player;
    #endregion

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    #region �÷��̾� �⺻ ���� ��Ʈ�ڽ�
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag(Define.StringTag.Enemy.ToString()))
        {
            print(target.name + " ���� " + player.DefaultAttackDamage + " �ο� ");
            target.GetComponent<Enemy>().TakeDamage(player.DefaultAttackDamage);
        }
    }
    #endregion
}
