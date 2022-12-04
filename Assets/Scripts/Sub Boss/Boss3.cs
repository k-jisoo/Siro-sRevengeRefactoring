using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3 : Enemy
{
    protected int Boss = 1 << 14;
    public enum BossState
    {
        IDLE_STATE,
        TELPO_STATE,
        ATTACK1_STATE,
        ATTACK2_STATE,
        Dead_STATE
    };
    Vector2 dir;
    BossState state = BossState.IDLE_STATE;
    bool isIdle = true;
    bool isTelpo = false;
    bool isAttack1 = false;
    bool isAttack2 = false;
    bool isDie = false;
    bool isStart = false;
    [SerializeField] GameObject Telpo;
    [SerializeField] GameObject Fire;
    [SerializeField] GameObject Portalpref;
    GameObject myInstance;
    float fireDelay = 1f;
    float attackDelay = 3f;
    private int skillDamage = 5;
    private void Start()
    {
        base.Start();
        Invoke(nameof(GetBossLayer), 4.5f); 

    }
    private void GetBossLayer()
    {
        gameObject.layer = 14;
        gameObject.tag = Define.StringTag.Enemy.ToString();
        isStart = true;
    }
    void Update()
    {
        dir = (playerTarget.transform.position - transform.position);
        Fsm();
        ChangeOrder();
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

    public override void TakeDamage(int newDamage)
    {
        base.TakeDamage(newDamage);
        Managers.StageManager.IsBossAlive(Hp);
        Managers.UI.UpdateBossHpSlider(Hp, MaxHp);
        EnemyAnimator.SetBool("isHit", true);
    }
    private void HurtToIdle()
    {
        EnemyAnimator.SetBool("isHit", false);
        state = BossState.IDLE_STATE;
    }
    void Fsm()
    {
        switch (state)
        {
            case BossState.IDLE_STATE:
                Idle();
                break;
            case BossState.TELPO_STATE:
                telpo();
                break;
            case BossState.ATTACK1_STATE:
                Attack1();
                break;
            case BossState.ATTACK2_STATE:
                Attack2();
                break;
            case BossState.Dead_STATE:
                break;
        }
    }
    private void Idle()
    {
        if (isIdle && isStart)
        {
            isIdle = false;
            Invoke(nameof(IdleToAttack), attackDelay);
        }
       
    }

    private void IdleToAttack()
    {
        
        isIdle = true;
        int result = Random.Range(0, 2);
        if (result == 0)
        {
            state = BossState.TELPO_STATE;
        }
        else
        {
            state |= BossState.ATTACK2_STATE;
      
        }
    }
    private void telpo()
    {
        if (!isTelpo&&!isDie)
        {
            isTelpo = true;
            StartCoroutine(TelpoToPlayer());
        }
    }

    IEnumerator TelpoToPlayer()
    {
        TelpoSound();
        yield return new WaitForSeconds(0.5f);
        GameObject projectile = MemoryPoolManager.GetInstance().OutputGameObject(Telpo,
                                                                                    "SubBoss/"+Telpo.name,
                                                                                    new Vector2(transform.position.x - 1.07f, transform.position.y + 1.33f),
                                                                                    Quaternion.identity);
        projectile.SetActive(true);
        int result = Random.Range(0, 2);
        if(result == 0)
        {
            transform.position = new Vector2(playerTarget.transform.position.x + 3, playerTarget.transform.position.y - 1);
            gameObject.transform.rotation =Quaternion.Euler(0,0, 0);
        }
        else
        {
            transform.position = new Vector2(playerTarget.transform.position.x -3, playerTarget.transform.position.y - 1);
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        
        GameObject projectile2 = MemoryPoolManager.GetInstance().OutputGameObject(Telpo,
                                                                                     "SubBoss/" + Telpo.name,
                                                                                    new Vector2(transform.position.x -2.71f, transform.position.y + 1.33f),
                                                                                    Quaternion.identity);
        projectile2.SetActive(true);
        state = BossState.ATTACK1_STATE;
        isTelpo = false;
    }

    private void Attack1()
    {
        if (!isAttack1)
        {
            attackDelay = 3f;
            StartCoroutine(Attack_1());
            isAttack1 = true;
        }
    }

    IEnumerator Attack_1()
    {
        yield return new WaitForSeconds(1f);
        EnemyAnimator.SetBool("isAttack1", true);
    }

    private void Attack1ToIdle()//공경1애니 마지막에 넣기
    {
        EnemyAnimator.SetBool("isAttack1", false);
        state = BossState.IDLE_STATE;
        isAttack1 = false;
    }
    
    private void Attack2()
    {
        EnemyAnimator.SetBool("isAttack2", true);
        if (!isAttack2&&!isDie)
        {
            attackDelay = 10f;
            isAttack2 = true;
            StartCoroutine(FireAttack());
        }

    }
   
    IEnumerator FireAttack()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject projectile = MemoryPoolManager.GetInstance().OutputGameObject(Fire,
                                                                                   "SubBoss/"+Fire.name,
                                                                                   new Vector2(playerTarget.transform.position.x, playerTarget.transform.position.y-1),
                                                                                   Quaternion.identity);
            projectile.GetComponent<Projectile>().ProjectileInit(Define.StringTag.Player, Vector2.zero, skillDamage);
            projectile.SetActive(true);
            yield return new WaitForSeconds(fireDelay);
        }
        isAttack2 = false;
    }
    private void Attack2ToIdle()//공격2애니 마지막에 넣기
    {
        EnemyAnimator.SetBool("isAttack2", false);
        state = BossState.IDLE_STATE;
    }
    protected override void OnDead()
    {
        state = BossState.Dead_STATE;
        isDie = true;
        base.OnDead();
        Managers.UI.bossSlider.gameObject.SetActive(false);
        EnemyAnimator.SetTrigger("isDie");
    }

    private void destory()//죽는 애니 마지막에 넣기
    {
        StartCoroutine(FadeAway());
    }
    IEnumerator FadeAway()
    {
        while (SpriteRenderer.color.a > 0)
        {
            var color = SpriteRenderer.color;
            //color.a is 0 to 1. So .5*time.deltaTime will take 2 seconds to fade out
            color.a -= (.5f * Time.deltaTime);

            SpriteRenderer.color = color;
            //wait for a frame
            yield return null;
        }
        Invoke(nameof(SpawnPortal), 2f);
    }
    void SpawnPortal()
    {
        myInstance = Instantiate(Portalpref);
        myInstance.transform.position = new Vector2(transform.position.x-2f, transform.position.y+1f);
        Destroy(gameObject);
    }
    void AttackSound()
    {
        Managers.Sound.PlaySFXAudio("SubBoss/MP_swosh-sword-swing");
    }
    void DeadSound()
    {
        Managers.Sound.PlaySFXAudio("SubBoss/Boss3_Die_SFX");
    }
    void TelpoSound()
    {
        Managers.Sound.PlaySFXAudio("SubBoss/boss3_Telpo_SFX");
    }

}
