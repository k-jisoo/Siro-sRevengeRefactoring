using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public abstract class BasicMonsterController : Enemy
{
    float radius = 0.2f;
    bool noCoin = false;
    public GameObject coinPrefap;
    public AudioSource attackAudio;
    public float volume;

    public enum State
    {
        Run,
        Attack,
        Damage,
        Die
    }
    public State state;

    public float coolTime=-1.0f, skillTime = 2.0f;
    new SpriteRenderer renderer;

    
    new public void Start()
    {
        base.Start();
        renderer = GetComponent<SpriteRenderer>();
        attackAudio = GetComponent<AudioSource>();
        state = State.Run;
    }


    private void OnEnable()
    {
        state = State.Run;
        noCoin = false;
    }

    public void Update()
    {
        if (Managers.StageManager.isSpawnOkay == false) {
            noCoin = true;
            OnDead(); 
        }
        else
        {
            if (state == State.Run) Run();
            if (state == State.Attack) Attack();
        }
    }

    //달리기
    public void Run()
    {
        base.Move();
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
        if(collision.tag=="DamagedRadius")
            state = State.Run;
        coolTime = -1.0f;
        base.EnemyAnimator.SetTrigger("AttackToMove");
    }

    protected abstract void Attack();


    //데미지 받기
    //데미지 표시, 데미지 입은 애니메이션
    public override sealed void TakeDamage(int newDamage)
    {
        if (state == State.Die) return;
        EnemyRigidbody.velocity = Vector2.zero;
        renderer.color = new Color(255 / 255f, 0 / 255f, 0 / 255f, 255 / 255f);
        Managers.Sound.PlaySFXAudio("Monster/Damaged",null,0.4f,false);
        base.TakeDamage(newDamage);

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
            EnemyRigidbody.velocity = Vector2.zero;
            state = State.Die;
            return;
        }
        //state Run or Attack
        if (Physics2D.OverlapCircle(this.transform.position, radius, 1<<10) == true)
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
       // renderer.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
        yield return new WaitForSeconds(0.3f);
        renderer.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
    }


    //죽음
    //죽음 애니메이션,코인드랍,비활성화
    protected override void OnDead()
    {
        base.OnDead();
        base.EnemyCollider.enabled = false;
        EnemyRigidbody.velocity = Vector2.zero;
        base.EnemyAnimator.SetTrigger("Die");
        StartCoroutine(DieProcess());
    }
    IEnumerator DieProcess()
    {
        yield return new WaitForSeconds(1.0f);

        if (noCoin==false)
        {
            GameObject coin = MemoryPoolManager.GetInstance().OutputGameObject
                   (coinPrefap,
                    "Coin/" + coinPrefap.name,
                    transform.position,
                    Quaternion.identity);

            coin.SetActive(true);
        }

        Debug.Log("audio name : " + attackAudio.name);

        if (attackAudio.name != "BlueSkull(Clone)"||Managers.StageManager.ReturnKillCount() >= 0)
            Managers.StageManager.DecreaseKillCount();
       
        //캐릭터 정보에 킬카운트 넘겨주기

        base.EnemyCollider.enabled = true;
        
        renderer.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
        gameObject.SetActive(false);

    }

    private void OnDisable()
    {
        BasicStatInit();
        MemoryPoolManager.GetInstance().InputGameObject(gameObject);
    }
}