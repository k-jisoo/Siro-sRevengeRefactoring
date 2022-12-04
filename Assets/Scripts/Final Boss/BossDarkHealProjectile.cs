using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDarkHealProjectile : MonoBehaviour
{
    private Rigidbody2D DarkHealRigidbody;
    private Animator DarkHealAnimator;


    private void Awake()
    {
        DarkHealRigidbody = GetComponent<Rigidbody2D>();
        DarkHealAnimator = GetComponent<Animator>();
            
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.CompareTag(Define.StringTag.Player.ToString()))
        {
            
        }
    }
}
