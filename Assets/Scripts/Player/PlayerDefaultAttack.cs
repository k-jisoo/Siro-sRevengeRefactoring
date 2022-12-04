using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefaultAttack : MonoBehaviour
{
    #region 플레이어 정보 변수
    private Player player;
    #endregion

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    #region 플레이어 기본 공격 히트박스
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag(Define.StringTag.Enemy.ToString()))
        {
            print(target.name + " 에게 " + player.DefaultAttackDamage + " 부여 ");
            target.GetComponent<Enemy>().TakeDamage(player.DefaultAttackDamage);
        }
    }
    #endregion
}
