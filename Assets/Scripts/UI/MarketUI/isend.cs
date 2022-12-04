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
        {//블랙 아웃 애니메이션이 실행되는 조건
            anim.SetTrigger("isEnd");
        }
    }

}
