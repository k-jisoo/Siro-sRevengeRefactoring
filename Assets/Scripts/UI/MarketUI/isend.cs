using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isend : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        if (true)
        {//�� �ƿ� �ִϸ��̼��� ����Ǵ� ����
            anim.SetTrigger("isEnd");
        }
    }

}
