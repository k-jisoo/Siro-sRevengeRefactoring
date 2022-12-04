using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : Enemy
{
    protected int Boss = 1 << 14;
    public enum BossState
    {
        IDLE_STATE, 
        MOVE_STATE,
        ATTACK_STATE
    }
    Vector2 dir;
    Vector2 attackDir;
    BossState state = BossState.IDLE_STATE;
    int attackCnt;
    bool isMove=false;
    bool isAttack=false;
    bool isDie = false;
    bool isStart = false;
    float idleDelay = 3f;
    float moveTime = 3f;
    int skillDamage = 3;
    int skillSpeed = 10;
    [SerializeField] GameObject FireBall;
    [SerializeField] Transform FireBallTransform;
    [SerializeField] Material HitEffectMaterial;     // 피격 시 머티리얼
    [SerializeField] Material orignalMaterial;
    [SerializeField] GameObject Portalpref;
    GameObject myInstance;

    private void Start()
    {
        base.Start();
        Invoke(nameof(GetBossLayer),4.5f);
    }
    void Update()
    {
        attackDir= (playerTarget.transform.position - FireBallTransform.position);
        dir = (playerTarget.transform.position - transform.position);
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
        }
    }
    void Idle()
    {
        if (!isMove && isStart)
        {
            StartCoroutine(IdleToMove());
            isMove = true;
        }
    }
   
    IEnumerator IdleToMove()
    {
        yield return new WaitForSeconds(idleDelay);
        state = BossState.MOVE_STATE;
        EnemyAnimator.SetBool("isMove", true);
        isMove = false;
    }

    new void Move()
    {
        if (!isDie)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTarget.transform.position, MoveSpeed * Time.deltaTime);
            ChangeDir();
        }
        if (!isAttack)
        {
            StartCoroutine(MoveToAttack());
            isAttack = true;
        }
    }
    IEnumerator MoveToAttack()
    {
        yield return new WaitForSeconds(moveTime);
        state = BossState.ATTACK_STATE;
        EnemyAnimator.SetBool("isAttack", true);
        isAttack = false;
    }
    void Attack()
    {
        if (!isDie)
        {
            ChangeDir();
            EnemyAnimator.SetBool("isHit", false);
        }
    }

    void MakeFireBall()
    {
        GameObject projectile = MemoryPoolManager.GetInstance().OutputGameObject(FireBall,
                                                                                 "SubBoss/"+FireBall.name,
                                                                                 new Vector2(FireBallTransform.position.x,FireBallTransform.position.y),
                                                                                 Quaternion.Euler(0, 0, Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg));
        projectile.GetComponent<Projectile>().ProjectileInit(Define.StringTag.Player, attackDir.normalized, skillDamage, skillSpeed);
        projectile.SetActive(true);
    }
    void AttackToIdle()
    {
        attackCnt++;
        if(attackCnt == 5)
        {
            attackCnt = 0;
            EnemyAnimator.SetBool("isAttack", false);
            EnemyAnimator.SetBool("isMove", false);
            state = BossState.IDLE_STATE;
        }
    }
    public override void TakeDamage(int newDamage)
    {
        base.TakeDamage(newDamage);
        EnemyAnimator.SetBool("isHit", true);
        Managers.UI.UpdateBossHpSlider(Hp, MaxHp);
        Managers.StageManager.IsBossAlive(Hp);
        StartCoroutine(SwitchMaterial());
    }

    private IEnumerator SwitchMaterial()
    {
        SpriteRenderer.material = HitEffectMaterial;
        yield return new WaitForSeconds(0.1f);
        SpriteRenderer.material = orignalMaterial;
    }
    private void HurtToIdle()
    {
        EnemyAnimator.SetBool("isHit", false);
    }
    protected override void OnDead()
    {
        isDie = true;
        base.OnDead();
        Managers.UI.bossSlider.gameObject.SetActive(false);
        EnemyAnimator.SetTrigger("Die");
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
        Invoke(nameof(SpawnPortal), 1f);
    }

    void SpawnPortal()
    {
        myInstance = Instantiate(Portalpref);
        myInstance.transform.position = transform.position;
        Destroy(gameObject);

    }
    void FireSound()
    {
        Managers.Sound.PlaySFXAudio("SubBoss/fire-magic-6947");
    }
    void DeadSound()
    {
        Managers.Sound.PlaySFXAudio("SubBoss/저글링4");
    }
    
}
