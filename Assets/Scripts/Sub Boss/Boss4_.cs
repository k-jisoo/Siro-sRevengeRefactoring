using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss4_ : Enemy
{
    protected int Boss = 1 << 14;
    public enum BossState
    {
        IDLE_STATE,
        MOVE_STATE,
        ATTACK1_STATE,
        ATTACK2_STATE,
        Dead_STATE
    }
    Vector2 dir;
    float distance;
    BossState state = BossState.IDLE_STATE;
    bool isIdle = true;
    bool isDie = false;
    bool isStart = false;
    float attackDelay = 4f;
    float attackdistance = 3f;
    int attack2Cnt =0;
    int skillDamage = 3;
    [SerializeField] GameObject Spell;
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
    void Idle()
    {
        if (isIdle&& isStart)
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
            state = BossState.MOVE_STATE;
        }
        else
        {
            state = BossState.ATTACK2_STATE;
        }
    }


    new void Move()
    {
        if (!isDie)
        {
            EnemyAnimator.SetBool("isMove", true);
            ChangeDir();
            if (dir.x < 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerTarget.transform.position.x + 2.5f,
                    playerTarget.transform.position.y - 1f), MoveSpeed * Time.deltaTime);

            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerTarget.transform.position.x - 2.5f,
                    playerTarget.transform.position.y - 1f), MoveSpeed * Time.deltaTime);
            }

            if (distance < attackdistance && dir.normalized.y < 0.6f && dir.normalized.y > 0.2f)
            {
                state = BossState.ATTACK1_STATE;
            }
        }
        
    }

    void Attack1ToIdle()
    {
        state = BossState.IDLE_STATE;
        EnemyAnimator.SetBool("isAttack1", false);
        EnemyAnimator.SetBool("isMove", false);
    }

    void Attack2ToIdle()
    {
        attack2Cnt++;
        if(attack2Cnt == 5)
        {
            state = BossState.IDLE_STATE;
            EnemyAnimator.SetBool("isAttack2", false);
            attack2Cnt = 0;
        }
        GameObject projectile = MemoryPoolManager.GetInstance().OutputGameObject(Spell,
                                                                                   "SubBoss/"+Spell.name,
                                                                                   new Vector2(playerTarget.transform.position.x, playerTarget.transform.position.y +3.8f),
                                                                                   Quaternion.identity);
        projectile.GetComponent<Projectile>().ProjectileInit(Define.StringTag.Player, Vector2.zero, skillDamage);
        projectile.SetActive(true);

    }
    void Attack1()
    {
        EnemyAnimator.SetBool("isAttack1", true);
        EnemyAnimator.SetBool("isHit", false);
    }
    void Attack2()
    {
        EnemyAnimator.SetBool("isAttack2", true);
        EnemyAnimator.SetBool("isHit", false);
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
    }
    protected override void OnDead()
    {
        isDie = true;
        state = BossState.Dead_STATE;
        base.OnDead();
        Managers.UI.bossSlider.gameObject.SetActive(false);
        EnemyAnimator.SetTrigger("Die");
        DeadSound();
    }

    private void destory()//죽는 애니 마지막에 넣기
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
    void Attack1Sound()
    {
        Managers.Sound.PlaySFXAudio("SubBoss/MP_swosh-sword-swing");
    }
    void DeadSound()
    {
        Managers.Sound.PlaySFXAudio("SubBoss/boss4_Die_SFX");
    }
}
