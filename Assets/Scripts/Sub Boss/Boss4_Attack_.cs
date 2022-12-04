using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Boss4_Attack_ : MonoBehaviour
{
    Boss4_ boss4;
    private void Start()
    {
        boss4 = GetComponentInParent<Boss4_>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.StringTag.Player.ToString()))
        {
            collision.GetComponent<Player>().TakeDamage(boss4.DefaultAttackDamage);
        }
    }
}
