using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : Enemy
{
    protected int Boss = 1 << 14;
    protected int DontDamaged = 1 << 15;
    public enum BossState
    {
        IDLE_STATE,
        MOVE_STATE,
        ATTACK_STATE,
        HIDE_STATE
    };
    float distance;
    float moveDistance = 15f;
    float attackDistance = 2.5f;
    float attackDelay = 2f;
    Vector2 dir;
    BossState state = BossState.IDLE_STATE;
    bool isAttack = true;
    bool isHurt = false;
    bool isDie = false;
    bool isStart = false;
    int attackCnt = 0;
    bool isFadeout = true;
    bool isHide = false;
    bool isIdle = true;
    [SerializeField] GameObject Portalpref;
    GameObject myInstance;


    private void Start()
    {
        base.Start();
        Invoke(nameof(GetBossLayer), 4.5f);
    }
    void Update()
    {
        dir = (playerTarget.transform.position - transform.position);
       distance = dir.magnitude; 
        Fsm();
        ChangeOrder();
    }
    private void GetBossLayer()
    {
        gameObject.layer = 14;
        gameObject.tag = Define.StringTag.Enemy.ToString();
        isStart = true;

    }
    void ChangeOrder()
    {
        if (dir.y < 0)
        {
            SpriteRenderer.sortingOrder = 3;
        }
        else
        {
            SpriteRenderer.sortingOrder = 5;
        }
    }
    private void ChangeDir()
    {
        if (dir.x < 0)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    void Fsm()
    {
        switch (state)
        {
            case BossState.IDLE_STATE:
                Idle();
                break;
            case BossState.MOVE_STATE:
                Move();
                break;
            case BossState.ATTACK_STATE:
                Attack();
                break;
            case BossState.HIDE_STATE:
                Hide();
                break;
        }
    }

    void Idle()
    {
        if (isIdle&&isAttack&&!isDie&&isStart) 
        {
            isIdle = false;
            StartCoroutine(IdleToMove());
        }
        EnemyAnimator.SetBool("isRun", false);
    }

    IEnumerator IdleToMove()
    {
        yield return new WaitForSeconds(2f);
        state = BossState.MOVE_STATE;
        isIdle = true;
    }


    private new  void Move()
    {
        if (!isHurt)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTarget.transform.position, MoveSpeed * Time.deltaTime);
            ChangeDir();
        }
       
        EnemyAnimator.SetBool("isRun", true);
        if (distance <= attackDistance)
        {
            state = BossState.ATTACK_STATE;
        }
    }

    private void Attack()
    {
        if (isAttack)
        {
            StartCoroutine(AttackDelay());
        }
        ChangeDir();
    }
    public override void TakeDamage(int newDamage)
    {
        print("데미지 받음");
        Managers.StageManager.IsBossAlive(Hp);
        Managers.UI.UpdateBossHpSlider(Hp, MaxHp);
        base.TakeDamage(newDamage);
        Hurt();
    }

  

    protected override void OnDead()
    {
        base.OnDead();
        Managers.UI.bossSlider.gameObject.SetActive(false);
        isDie = true;
        EnemyAnimator.SetTrigger("isDie");
        Deadsound();
    }
    void DeadProcess()//Die애니메이션에 넣음
    {
        var color = SpriteRenderer.color;
        color.a = 0;
        SpriteRenderer.color = color;
        Invoke(nameof(SpawnPortal), 2f);
    }
    void SpawnPortal()
    {
        myInstance = Instantiate(Portalpref);
        myInstance.transform.position = transform.position;
        Destroy(gameObject);

    }

    private void Hurt()
    {
        EnemyAnimator.SetBool("isDamaged", true);
        isHurt = true;
    }
    
    void  HurtToIdle()
    {
        EnemyAnimator.SetBool("isDamaged", false);
        isHurt = false;
        state = BossState.IDLE_STATE;
    }


    IEnumerator AttackDelay()
    {
        isAttack = false;
        EnemyAnimator.SetBool("isRun", false);
       // yield return new WaitForSeconds(0.1f);//0.2초 동안 idle
        EnemyAnimator.SetBool("isAttack", true);
        
        yield return new WaitForSeconds(attackDelay);//공격 딜레이 
        isAttack = true;
    }
    void AttackToIdle()//attack애니메이션 마지막에 넣음
    {
        attackCnt++;
        EnemyAnimator.SetBool("isAttack", false);
        StartCoroutine(AfterDelay());//공격 후 딜레이
        //EnemyCollider.isTrigger = false;
    }
    IEnumerator AfterDelay()
    {
        if(attackCnt == 2)
        {
            state = BossState.HIDE_STATE;
            yield return new WaitForSeconds(0.8f);
            attackCnt = 0;
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            state = BossState.IDLE_STATE;
        }
       
    }
    private void Hide()
    {
        if (isHide&!isDie)
        {
            int result = Random.Range(0, 2);
            if (result == 0)
            {
                transform.position = new Vector2(playerTarget.transform.position.x + 3f, playerTarget.transform.position.y);
            }
            else
            {
                transform.position = new Vector2(playerTarget.transform.position.x - 3f, playerTarget.transform.position.y);
            }
            
        }
        if (isFadeout)
        {
            isFadeout = false;
            StartCoroutine(FadeOut());
        }
         
       
    }
    IEnumerator FadeOut()
    {
        gameObject.tag = "Untagged";
        gameObject.layer = 15;
        FadeOutsound();
        while (SpriteRenderer.color.a > 0)
        {
            var color = SpriteRenderer.color;
            //color.a is 0 to 1. So .5*time.deltaTime will take 2 seconds to fade out
            color.a -= (3f * Time.deltaTime);
            if(color.a < 0)
            {
                color.a = 0;
            }
            SpriteRenderer.color = color;
            //wait for a frame
            yield return null;
        }
        isHide = true;
        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeIn()
    {
        gameObject.tag = "Enemy";
        gameObject.layer = 14;
        FadeINsound();
        state = BossState.ATTACK_STATE;
        while (SpriteRenderer.color.a < 1)
        {
            var color = SpriteRenderer.color;
            //color.a is 0 to 1. So .5*time.deltaTime will take 2 seconds to fade out
            color.a += (3f * Time.deltaTime);
         
            SpriteRenderer.color = color;
            //wait for a frame
            yield return null;
        }
        isFadeout = true;
        isHide = false;

    }
    void Attacksound()
    {
        Managers.Sound.PlaySFXAudio("SubBoss/boss2_Attack_SFX");
    }
    void FadeOutsound()
    {
        Managers.Sound.PlaySFXAudio("SubBoss/FadeOut_SFX");
    }
    void FadeINsound()
    {
        Managers.Sound.PlaySFXAudio("SubBoss/FadeIn_SFX");
    }
    void Deadsound()
    {
        Managers.Sound.PlaySFXAudio("SubBoss/pzeDth00");
    }
}
