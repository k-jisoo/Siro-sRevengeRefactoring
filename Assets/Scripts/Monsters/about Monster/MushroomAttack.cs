using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.AI;

public class MushroomAttack : BasicMonsterController
{
    float mushroomAttackRadius = 8.0f;
    Collider2D c;
    float time = 0;
    SpriteRenderer mushroomRenderer;
    Vector3 bombPosition;
    bool isAttack = false;
    protected override void Attack()
    {
        if (isAttack == true)
        {
            return;
        }
        isAttack = true;
        EnemyRigidbody.velocity = Vector3.zero;
        mushroomRenderer = GetComponent<SpriteRenderer>();
        c = GetComponent<Collider2D>();
        base.EnemyAnimator.SetTrigger("Attack");
        Managers.StageManager.monsterCounter--;
        bombPosition = this.transform.position;
        base.EnemyAnimator.SetTrigger("Smoke");
        transform.position = bombPosition;
        c.isTrigger = true;
        Hp = 10000000;
        string name = base.attackAudio.ToString().Substring(0, base.attackAudio.ToString().Length - 33);
        Debug.Log(name + "_Attack");
        Managers.Sound.PlaySFXAudio("Monster/" + name + "_Attack", base.attackAudio, volume, false);
        StartCoroutine(AttackProcess());
    }

    IEnumerator AttackProcess()
    {
        c.enabled = false;
        transform.position = bombPosition;
        this.transform.localScale = (new Vector3(10, 10, 0));
        mushroomRenderer.color = new Color(100/255f, 55/255f, 140/255f, 220/255f);
        if (Physics2D.OverlapCircle(this.transform.position, mushroomAttackRadius, 1<<10) == true)
        {
            base.DefaultAttack();
        }
        yield return new WaitForSeconds(1.0f);

        if (time > 5)
        {

            this.transform.localScale = (new Vector3(4, 4, 0));
            mushroomRenderer.color = new Color(255, 255, 255, 255);
            c.enabled = true;
            time = 0;
            gameObject.SetActive(false);
        }
        else
        {
            time += 1;
            StartCoroutine("AttackProcess");
        }
    }

}