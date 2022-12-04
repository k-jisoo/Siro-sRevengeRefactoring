using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamController :MonoBehaviour
{
    private int damage = 35;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.StringTag.Player.ToString()))
        {
             collision.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }
}
