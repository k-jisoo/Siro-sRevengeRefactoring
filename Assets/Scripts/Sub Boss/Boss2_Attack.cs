using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Boss2_Attack : MonoBehaviour
{
    Boss2 boss2;
    private void Start()
    {
        boss2 = GetComponentInParent<Boss2>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.StringTag.Player.ToString()))
        {
            collision.GetComponent<Player>().TakeDamage(boss2.DefaultAttackDamage);
        }
    }
}
