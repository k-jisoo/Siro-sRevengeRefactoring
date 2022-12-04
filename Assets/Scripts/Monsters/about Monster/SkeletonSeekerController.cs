using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class SkeletonSeekerController : Enemy
{
    float radius = 0.2f;
    //public GameObject coinPrephab;
    public enum State
    {
        Ready,
        Run,
        Attack,
        Damage,
        Die
    }
    public State state;

    public float coolTime = -1.0f, skillTime = 2.0f;
    new SpriteRenderer renderer;
    float attackRadius=2.5f;

    public int minKillCount;
    public int maxKillCount;

    new public void Start()
    {
        base.Start();
        renderer = GetComponent<SpriteRenderer>();
        state = State.Ready;

        //Boss쪽에서 아래 함수를 불러주면 완료
         //Ready();
    }

    public void Ready()
    {
        base.EnemyAnimator.SetTrigger("RunSpawnToWalk");
        this.GetComponent<Animator>().enabled = true;
        Debug.Log("레디함수 들어옴");
        StartCoroutine(ReadyProcess());
        state = State.Run;
    }

    IEnumerator ReadyProcess()
    {
        yield return new WaitForSeconds(2.0f);
    }

    public void Update()
    {
        if (state == State.Run) Run();
        if (state == State.Attack) Attack();
    }

    //달리기
    public void Run()
    {
        base.Move();
        this.GetComponent<Collider2D>().enabled = true;
        if ((playerTarget.gameObject.transform.position.x - this.transform.position.x) < 0)
            renderer.flipX = true;
        else renderer.flipX = false;
    }

    //공격
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DamagedRadius"))
        {
            state = State.Attack;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "DamagedRadius")
            state = State.Run;
        coolTime = -1.0f;
        base.EnemyAnimator.SetTrigger("AttackToMove");
    }

    protected void Attack()
    {
        if (coolTime < 0)
        {
            base.EnemyAnimator.SetTrigger("Attack");
            StartCoroutine(AttackProcess());
            base.EnemyAnimator.SetTrigger("AttackToMove");
            coolTime = skillTime;
        }
        coolTime -= Time.deltaTime;
    }

    IEnumerator AttackProcess()
    {
        yield return new WaitForSeconds(1.0f);
    }

    void AttackPlayer()
    {
        if (Physics2D.OverlapCircle(this.transform.position, attackRadius , 1 << 10))
        {
            base.DefaultAttack();
        }

    }


    //데미지 받기
    //데미지 표시, 데미지 입은 애니메이션
    public override sealed void TakeDamage(int newDamage)
    {
        base.TakeDamage(newDamage);
        Managers.Sound.PlaySFXAudio("Final_Boss_SFX/04_sack_open_2");
        //Damage text
        GameObject floatingText = MemoryPoolManager.GetInstance().OutputGameObject
            (Managers.Resource.GetPerfabGameObject("UI/DamageText")
            , "UI/DamageText"
            , new Vector3(transform.position.x, transform.position.y)
            , Quaternion.identity);

        floatingText.GetComponent<FloatingText>().DamageText = newDamage.ToString();
        floatingText.SetActive(true);

        base.EnemyAnimator.SetTrigger("MoveToDamage");
        StartCoroutine(DamageProcess());
        if (base.Hp <= 0)
        {
            state = State.Die;
            return;
        }
        //state Run or Attack
        if (Physics2D.OverlapCircle(this.transform.position, radius, 1 << 10) == true)
        {
            state = State.Attack;
            base.EnemyAnimator.SetTrigger("DamageToAttack");
        }
        else
        {
            state = State.Run;
            base.EnemyAnimator.SetTrigger("DamageToMove");
        }
    }

    IEnumerator DamageProcess()
    {

        yield return new WaitForSeconds(1.0f);
    }


    //죽음
    //죽음 애니메이션,코인드랍,비활성화
    protected override sealed void OnDead()
    {
        base.OnDead();
        EnemyRigidbody.velocity = Vector2.zero;
        base.EnemyAnimator.SetTrigger("Die");
        StartCoroutine(DieProcess());
    }
    IEnumerator DieProcess()
    {
        Managers.Sound.PlaySFXAudio("Final_Boss_SFX/Beast_Bellow4");
        
        yield return new WaitForSeconds(1.0f);

        //int killCount = Random.Range(minKillCount, maxKillCount);
        //gameObject.SetActive(false);

        Destroy(gameObject);

    }
}
