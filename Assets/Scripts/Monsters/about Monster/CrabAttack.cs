using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class CrabAttack : BasicMonsterController
{
    float crabAttackRadius = 0.8f;
    public GameObject bomb1;
    public GameObject bomb2;
    public GameObject bomb3;
    public GameObject bomb4;
    public GameObject bomb5;
    public GameObject bomb6;

    protected override void Attack()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        if (base.coolTime < 0)
        {
            base.EnemyAnimator.SetTrigger("Attack");
            StartCoroutine(AttackProcess());
            base.EnemyAnimator.SetTrigger("AttackToMove");
            base.coolTime = base.skillTime;
        }
        base.coolTime -= Time.deltaTime;
    }

    IEnumerator AttackProcess()
    {
        bomb1.transform.position = transform.position + new Vector3(0, (float)0.1, 0);
        bomb2.transform.position = transform.position + new Vector3(0, (float)-0.3, 0);
        bomb3.transform.position = transform.position + new Vector3((float)0.2, (float)0.1, 0);
        bomb4.transform.position = transform.position + new Vector3((float)-0.2, (float)0.1, 0);
        bomb5.transform.position = transform.position + new Vector3((float)0.2, (float)-0.2, 0);
        bomb6.transform.position = transform.position + new Vector3((float)-0.2, (float)-0.2, 0);

        bomb1.SetActive(true);
        bomb2.SetActive(true);
        bomb3.SetActive(true);
        bomb4.SetActive(true);
        bomb5.SetActive(true);
        bomb6.SetActive(true);


        yield return new WaitForSeconds(4.0f);
    }

    void AttackPlayer()
    {
        string name = base.attackAudio.ToString().Substring(0, base.attackAudio.ToString().Length - 33);
        Debug.Log(name + "_Attack");
        Managers.Sound.PlaySFXAudio("Monster/" + name + "_Attack", base.attackAudio, base.volume, false);
        if (Physics2D.OverlapCircle(this.transform.position, crabAttackRadius, 1<<10) == true)
        {
            base.DefaultAttack();
        }

    }
}