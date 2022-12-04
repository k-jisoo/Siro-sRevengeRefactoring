using System.Collections;
using UnityEngine;

public class BasicAttack : BasicMonsterController
{
    public float attackRadius;
    protected override void Attack()
    {
        if (base.coolTime < 0)
        {
            base.EnemyAnimator.SetTrigger("Attack");

            string name = "";
            if (attackAudio.name == "BlueSkull(Clone)")
                name = "Blueskull";
            else name = base.attackAudio.ToString().Substring(0, base.attackAudio.ToString().Length - 26);
            Debug.Log(name + "_Attack");
            Managers.Sound.PlaySFXAudio("Monster/"+name+"_Attack", base.attackAudio, base.volume, false);
            StartCoroutine(AttackProcess());
            base.EnemyAnimator.SetTrigger("AttackToMove");
            base.coolTime = base.skillTime;
        }
        base.coolTime -= Time.deltaTime;
    }

    IEnumerator AttackProcess()
    {
        yield return new WaitForSeconds(1.0f);
    }

    void AttackPlayer()
    {
        if(Physics2D.OverlapCircle(this.transform.position, attackRadius, 1<<10))
        {
            base.DefaultAttack();
        }

    }
}

