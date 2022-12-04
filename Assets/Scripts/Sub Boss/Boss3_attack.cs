using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3_attack : MonoBehaviour
{
     Boss3 boss3;
    private void Start()
    {
        boss3 = GetComponentInParent<Boss3>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.StringTag.Player.ToString()))
        {
            collision.GetComponent<Player>().TakeDamage(boss3.DefaultAttackDamage);
        }
    }
}
