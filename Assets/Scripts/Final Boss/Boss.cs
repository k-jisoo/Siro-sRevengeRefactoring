using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Boss : Enemy
{
    

    #region ���� ��� ���
    readonly float BOSS_PROJECTILE_SKULL_SPEED = 10f;       //�⺻���� ����ü �ӵ�
    readonly float BOSS_PATTERN_DARK_HEAL_COUNT = 30f;      //��ũ�� ������ ���� ��Ÿ�� ���� ����
    readonly int BOSS_TEMP_HP = 100;                        //��ũ�� ���� �� ���Ǵ� �ӽ�ü��.
    #endregion             

    #region ���� ���� ��� �ø�������� �ʵ� 
    [SerializeField] [Range(0f, 50f)] float contactDistance;    //������ �����Ÿ� �ʱⰪ 10
    [SerializeField] GameObject finalBossSkull;             //�⺻���� ������
    [SerializeField] GameObject finalBossRuinStk;           //���ν�Ʈ����ũ ���� ������
    [SerializeField] GameObject finalBossDarkHeal;          //��ũ�� ���� ��¡ ������
    [SerializeField] GameObject finalBossDarkHealFailed;    //��ũ�� ���� ���� ������
    [SerializeField] GameObject finalBossBindEye;           //���ε� ���� �� �÷��̾� ���� ������
    [SerializeField] GameObject finalBossBindVineFail;      //���ε� ���� ���� ������ 
    [SerializeField] GameObject finalBossBindVineSucess;    //���ε� ���� ���� ������
    [SerializeField] GameObject finalBossInputSoul;         //Seeker ��ȯ �� ����� ������ 
    [SerializeField] GameObject[] keyListObject;            //���ε� ���� Ű ��� ��� ������
    [SerializeField] Material originalMaterial;
    [SerializeField] Material hurtMaterial;
   // [SerializeField] GameObject player;         //�÷��̾� ������ 
    [SerializeField] GameObject seekerPrefab;

    Player player;
    


    #endregion

    #region Destroyó���� ����� GameObject ����
    GameObject darkHealA;
    GameObject darkHealB;
    GameObject bindEye;
    GameObject bindVineSucess;
    GameObject bindVineFail;
    GameObject skeletonSeekrer;
    #endregion

    #region Pattern1_DefaultAttack_Flying Skull ���� ��������



    #endregion

    #region Pattern2_Dark Heal ���� ��������
    private float darkHealCheckTimer = 0.0f;    //��ũ�� �ӽ�ü�� �ı��ð��� �����ϴ� ���� 
    private int darkHealTempHp;                 //��ũ�� �ӽ�ü�� ������� �����ϴ� ����
    #endregion

    #region Pattern3_Ruin Strike ���� ��������
    private int ruinStrikeQty;                  //���ν�Ʈ����ũ ���� ���� ����
    private int ruinStrikeCheck = 0;
    #endregion

    #region Pattern4_Summon Skeleton ���� ��������
    private int sumnSkeletonCheck = 0;
    #endregion

    #region Pattern5_Bind ���� ��������
    private int bindCheck = 0;
    GameObject[] iconDestroy = new GameObject[6];           //Ű���� �������� �ѹ��� �����ϱ� ���� �������� 
    GameObject[] qteGameObjectArray = new GameObject[6];    //Ű���� ������ ������Ʈ�� ��� ���� ����
    char[] qteCharArray = new char[6];                      //Object�迭 ������ Char�������� ��ȯ�ϱ� ���� ����
    List<char> inputList = new List<char>();                //�÷��̾��� �Է� ���� �����ϱ� ���� ����
    private float x = -8f;                                  //Ű���� ������ ���� X��ǥ ���� �� ����. ��������
    private bool qteCheck = true;                          //QTE�� �÷��̾ �����Ѵٸ� true, ���и� false
    #endregion

    #region ��Ÿ ��������
    BossFSM bossFSM;
    Rigidbody2D bossRigidBody;
    Animator bossAnimator;
    SpriteRenderer bossSpriteRenderer;
    Transform targetIsPlayer;                   //�÷��̾��� ��ġ���� ����
    private float scaleX;
    private Vector2 dir;
    private float bossHpPercentage=100f;            //������ ü���� �ۼ�Ƽ���� ��Ÿ�� �����ϴ� ����
    private float patternCheckTimer = 0.0f;     //�����ð��� ������ Ư�� ������ �����Ű�� ���� �� ���
    private bool patternCheck = false;          // == isgod. true�̸� ������. 
    private bool isStart = false;
    #endregion

    [SerializeField] Spawner_SkeletonSeeker SkeletonSeekerSpawner;

    /// <summary>
    /// Ŭ������ ���� ��Ű�� �� �ʿ��� ������ ��� ������
    /// </summary>
    new void Start()
    {
        base.Start();
        player = Managers.StageManager.Player;
        bossFSM = new BossFSM(this);
        scaleX = transform.localScale.x;
        targetIsPlayer = Managers.StageManager.Player.transform;
        bossAnimator = GetComponent<Animator>();
        bossRigidBody = GetComponent<Rigidbody2D>();
        bossSpriteRenderer = GetComponent<SpriteRenderer>();
        darkHealTempHp = BOSS_TEMP_HP;
        
        bossFSM.bossState = Define.BossState.CASTING_STATE; //����ȿ�� ���
 
    }

    #region ���� ������ ����Ǹ� Move ���·� ��ȯ
    public void BossSetBattle()
    {
        Invoke(nameof(GetBossLayer), 4f);
    }

    private void GetBossLayer()
    {
        gameObject.layer = 14;
        isStart = true;
        bossFSM.bossState = Define.BossState.MOVE_STATE;
    }
    #endregion


    void Update()
    {
        if (!isStart) return;

        SwitchSpriteImageDir(transform);
        if (bossFSM != null) bossFSM.Update();                      //������ STATE���� ���� ���� ������. 
        if (bossFSM.bossState != Define.BossState.CASTING_STATE)    //������ ���� ���� ���� ����üũŸ�̸Ӹ� �������� ����
        {
            patternCheckTimer += Time.deltaTime;
        }
        if (patternCheckTimer > BOSS_PATTERN_DARK_HEAL_COUNT)       //n�ʸ��� ��ũ�� ���� ����
        {
            bossFSM.bossState = Define.BossState.PATTERN_DARKHEAL_STATE;
            patternCheckTimer = 0;
        }
    }

    #region ���� �⺻ ������Ʈ, ���� ���� �Լ���
    /// <summary>
    /// ������ ���� �����Ÿ��� ���� �����ϰų� �̵���Ŵ
    /// </summary>
    public void Move()
    {
        //���� ������ �÷��̾��� �Ÿ����̰� �̸������� �����Ÿ����� ũ�ٸ� �÷��̾ ����, �ƴ϶�� ���� STATE�� ����
        if (Vector2.Distance(transform.position, targetIsPlayer.position) > contactDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetIsPlayer.position, MoveSpeed * Time.deltaTime);
            dir = (player.transform.position - transform.position).normalized;
        }
        else
        {
            bossFSM.bossState = Define.BossState.ATTACK_STATE;
        }
    }
    /// <summary>
    /// ������ ���ݻ����Ÿ��� ���� �÷��̾ �����ϰų� MOVE_STATE�� �����Ŵ
    /// </summary>
    public void Attack()
    {
        if (Vector2.Distance(transform.position, targetIsPlayer.position) > contactDistance)
        {
            bossFSM.bossState = Define.BossState.MOVE_STATE;
        }
        else if (Vector2.Distance(transform.position, targetIsPlayer.position) < contactDistance)
        {
            SetAnimationTrigger("DefaultAttack");
            bossRigidBody.velocity = Vector2.zero;
            //EnemyRigidbody.velocity = Vector2.zero;
        }
    }
    /// <summary>
    /// ������ �⺻���� �� �ذ� ����ü�� �����ϴ� �Լ�
    /// </summary>
    private void CreateBossProjectileSkull()
    {
        dir = (player.transform.position - transform.position).normalized;
        Managers.Sound.PlaySFXAudio("Final_Boss_SFX/Final Boss Basic");
        GameObject projectile = MemoryPoolManager.GetInstance().OutputGameObject(finalBossSkull,
                                                                                "Final_Boss_Skill/"+ finalBossSkull.name,
                                                                                transform.position,
                                                                                Quaternion.identity);
        SwitchSpriteImageDir(projectile.transform);
        projectile.GetComponent<Projectile>().ProjectileInit(Define.StringTag.Player, dir, DefaultAttackDamage, BOSS_PROJECTILE_SKULL_SPEED);
        projectile.SetActive(true);
    }
    /// <summary>
    /// �÷��̾�� �������� ���� �� ó���Ǵ� �Լ�
    /// </summary>
    public override sealed void TakeDamage(int newDamage)
    {
        if (patternCheck)              //���� ���� ���̶� ���� ó���� �ؾ��� ��
        {
            return;
        }
        if (bossFSM.runDarkHeal)       //��ũ�� ���� �������� ���� �÷��̾ ���ϴ� �������� �ӽ�ü�� �������� ���� �ϵ���.
        {
            darkHealTempHp -= newDamage;
            Hurt();
            return;
        }
        Hurt();
        base.TakeDamage(newDamage);
        Managers.UI.UpdateBossHpSlider(Hp, MaxHp);
        bossHpPercentage = (float)Hp / (float)MaxHp * 100;        //���� ü�� ������ �ִ�ü�� ���ϱ� ��
        print("������ ��� ó�� �� ���� ���� ü��"+ bossHpPercentage);
        //Managers.StageManager.IsBossAlive(Hp)
        //Managers.UI.UpdateBossHpSlider(Hp, MaxHp);

        RunPattern(bossHpPercentage);  //���� ���Ե� ������ ü���� ������ ���� ���Ѿ� �Ǵ� ü���̶�� �׿� �°� ������ ����.
        if(bossHpPercentage <= 0f) 
        {
            OnDead();
        }
    }
    /// <summary>
    /// �ǰ�ȿ�� ��� �Լ� 
    /// </summary>
    public void Hurt()
    {
        StartCoroutine(SwitchMaterial());
    }
    /// <summary>
    /// �ǰ�ȿ���� �����ð� ������Ŵ. (��������Ʈ�� ������� ����)
    /// </summary>
    IEnumerator SwitchMaterial()
    {
        Managers.Sound.PlaySFXAudio("Final_Boss_SFX/11_human_damage_3");
        bossSpriteRenderer.material = hurtMaterial;
        yield return new WaitForSeconds(0.1f);
        bossSpriteRenderer.material = originalMaterial;
    }
    /// <summary>
    /// ������ ����ü���� 0�̵Ǹ� �����Ű�� �Լ�
    /// </summary>
    protected override void OnDead()
    {
        bossFSM.bossState = Define.BossState.DEAD_STATE;
        SetAnimationTrigger("RunDead");

    }
    public void Die()
    {
        bossFSM.bossState = Define.BossState.CASTING_STATE;
        Managers.Sound.PlaySFXAudio("Final_Boss_SFX/Death");
        Invoke(nameof(DelayDeadAnim), 2.5f);
    }

    private void DelayDeadAnim()
    {
        SceneManager.LoadScene("Ending");
    }
    /// <summary>
    /// ������ ü�¿� ���� �ش��ϴ� ����� �����Ű�� �Լ�
    /// </summary>
    /// <param name="hpPercentage"></param>
    public void RunPattern(float hpPercentage)
    {
        if ((hpPercentage <= 70f) && (hpPercentage > 15f) && (bindCheck == 0))
        {
            
            bossFSM.bossState = Define.BossState.PATTERN_BIND_STATE;
            
            //SetAnimationTrigger("RunBindMotion");
        }
        else if ((hpPercentage <= 50f) && (hpPercentage > 30f) && (sumnSkeletonCheck == 0))
        {
            bossFSM.bossState = Define.BossState.PATTERN_SUMNSKELETON_STATE;
            //SetAnimationTrigger("RunSumnSkeleton"); 
        }
        else if ((hpPercentage <= 40f) && (hpPercentage > 20f) && (ruinStrikeCheck == 0))
        {
            bossFSM.bossState = Define.BossState.PATTERN_RUINSTK_STATE;
            ruinStrikeQty = 6;
            //SetAnimationTrigger("RunRuinStk");
        }
        else if ((hpPercentage <= 20f) && (hpPercentage > 5f) && (ruinStrikeCheck == 1))
        {
            bossFSM.bossState = Define.BossState.PATTERN_RUINSTK_STATE;
            ruinStrikeQty = 9;
            //SetAnimationTrigger("RunRuinStk");
        }
        else if ((hpPercentage <= 15f) && (hpPercentage > 0.1f) && (bindCheck == 1))
        {
            bossFSM.bossState = Define.BossState.PATTERN_BIND_STATE;
            //SetAnimationTrigger("RunBindMotion");
        }
        else if ((hpPercentage <= 5f) && (hpPercentage > 0.1f) && (ruinStrikeCheck == 2))
        {
            bossFSM.bossState = Define.BossState.PATTERN_RUINSTK_STATE;
            ruinStrikeQty = 12;
            //SetAnimationTrigger("RunRuinStk");
        }
        else
        {
            return;
        }
    }
    #endregion

    #region ��ũ �� ���� ���� �Լ���
    /// <summary>
    /// ���� ��ũ �� ���� �Լ�
    /// 1�� 30�ʸ��� �ݺ��ǰ� 8�ʵ��� �������� ����
    /// ���� �������� 1,000 �̻� �ް� �ȴٸ� ������ �ִ� �������� �����(���� �÷��̾� �ٽ� ������)
    /// ���� �ִ� ü���� 15%�� ȸ���� 
    /// </summary>
    public void Pattern_DarkHeal()
    {
        bossFSM.bossState = Define.BossState.CASTING_STATE; //������Ʈ ����
        bossRigidBody.velocity = Vector2.zero;              //���� ��ǥ ����
        SetAnimationTrigger("RunDarkHealMotion");           //�ִϸ��̼� Ʈ���� ����
        RunDarkHeal();                                      //�����Լ� ����
        Managers.Sound.PlaySFXAudio("Final_Boss_SFX/LostArkSkillSfx1");
    }
    public void RunDarkHeal(){  StartCoroutine(DarkHealProcess());  }
    IEnumerator DarkHealProcess()
    {
        darkHealA = CreateSimpleAnimation(finalBossDarkHeal, this.gameObject.transform, 0, 3);
        while (true)
        {

            if (darkHealCheckTimer >= 7.0f && darkHealTempHp > 0)          //�ð� ī���Ͱ� 8�ʰ� ������ 1,000 ������ ���Ϸ� �޾��� ��
            {
                Hp += MaxHp * 15 / 100;     //��ü���� 15%��ŭ ����
                Managers.UI.UpdateBossHpSlider(Hp, MaxHp);
                break;
            }
            else if (darkHealTempHp <= 0)     //�ð� ī���Ͱ� 8�ʰ� ������ �ʰ� 1,000������ �̻����� �޾��� ��
            {
                darkHealB = CreateSimpleAnimation(finalBossDarkHealFailed, this.gameObject.transform, 0, 3);
                break;
            }
            Managers.Sound.PlaySFXAudio("Final_Boss_SFX/45_Charge_05", null, 0.5f, false);
            yield return new WaitForSeconds(1.0f);
            darkHealCheckTimer += 1.0f;
            print(darkHealCheckTimer);
        }
        Destroy(darkHealA);
        if (darkHealB != null) Destroy(darkHealB, 1.0f);
        darkHealTempHp = BOSS_TEMP_HP;
        bossFSM.runDarkHeal = false;
        PatternReset();
    }
    #endregion

    #region ���� ��Ʈ����ũ ���� ���� �Լ���
    /// <summary>
    /// ���� ���� ��Ʈ����ũ ���� �Լ�
    /// ������ ü���� 40%, 25%, 5%�� �Ǿ��� ������ ���� 
    /// ������ ���ǿ� ���� 4��,6,��,9���� ����ü�� ������ ��ġ�� ������
    /// </summary>
    public void Pattern_RuinStk()
    {
        if (ruinStrikeCheck == 0) ruinStrikeCheck++;
        else if (ruinStrikeCheck == 1) ruinStrikeCheck++;
        
            
        bossRigidBody.velocity = Vector2.zero;
        SetBossGodMode();
        RunRuinStk();
        bossFSM.bossState = Define.BossState.CASTING_STATE;
        Managers.Sound.PlaySFXAudio("Final_Boss_SFX/surrender");
    }
    /// <summary>
    /// ���ν�Ʈ����ũ �ڷ�ƾ�� �����Ű�� �Լ�
    /// </summary>
    private void RunRuinStk()
    {
        StartCoroutine(RuinStrikeProcess(ruinStrikeQty));
    }
    IEnumerator RuinStrikeProcess(int qty)
    {
        for (int i = 0; i < qty; i++)
        {
            SetAnimationTrigger("RunRuinStk");
            print(i + "�� �߻�");
            yield return new WaitForSeconds(0.7f);
        }
        
        PatternReset();
        SetBossGodMode();
    }
    private void CreateBossProjectileRuinStk()
    {
        Managers.Sound.PlaySFXAudio("Final_Boss_SFX/summon-meteor");
        GameObject projectile = MemoryPoolManager.GetInstance().OutputGameObject(finalBossRuinStk,
                                                                                "Final_Boss_Skill/"+ finalBossRuinStk.name,
                                                                                new Vector2(transform.position.x + Random.Range(-10f, 10f), transform.position.y + Random.Range(-10f, 10f)),
                                                                                Quaternion.identity);
        projectile.GetComponent<Projectile>().ProjectileInit(Define.StringTag.Player, Vector2.zero, 400);
        projectile.SetActive(true);
    }
    #endregion

    #region ��Ŀ���̷��� ��ȯ���� �Լ���
    /// <summary>
    /// ���� ���̷��� ��ȯ ���� �Լ�
    /// ������ ü���� 50%�� �Ǿ��� �� �����Ǹ� ��ü���� ����. 
    /// ���̷����� ���ݷ��� 20, �⺻ ���� ���ݹۿ� ����. ü���� ���� ���� ����. 
    /// </summary>
    public void Pattern_SummonSkeleton()
    {
        if (sumnSkeletonCheck == 0) sumnSkeletonCheck++;

        bossFSM.bossState = Define.BossState.CASTING_STATE;
        SetAnimationTrigger("RunSumnSkeleton");
        SetBossGodMode();

        skeletonSeekrer = CreateSimpleAnimation(finalBossInputSoul, seekerPrefab.transform, 0f, 3f);
        StartCoroutine(skeletonGrowl());
        PatternReset();
    }
    IEnumerator skeletonGrowl()
    {
        RunSeekerSpawnerGo();
        Managers.Sound.PlaySFXAudio("Final_Boss_SFX/DeepOne_Growl5");
        yield return new WaitForSeconds(1.5f);
        SetBossGodMode();   
        bossFSM.bossState = Define.BossState.MOVE_STATE;

    }

    private void RunSeekerSpawnerGo()
    {
        SkeletonSeekerSpawner.go(); 
    }
    #endregion

    #region ���ε� ���� ���� �Լ���
    /// <summary>
    /// ���� ���ε� ���� �Լ�
    /// ü�� 70%, 15%�� �Ǿ��� �� �����. 
    /// 4�� ���� 6���� Ŀ�ǵ带 �ľ� ��. 
    /// Quick Time Event�� ���ÿ� ����Ǹ� �̸� �����ϸ� 5�� �ӹ�. 
    /// </summary>
    public void Pattern_Bind()
    {
        bossFSM.bossState = Define.BossState.CASTING_STATE;
        print("false���� ");
        Managers.StageManager.Player.PlayerController.delevList.Clear();
        Managers.StageManager.Player.PlayerController.isAttackalble = false;
        Managers.StageManager.Player.PlayerController.bossDebuff = true;    
        print("false ���ְ� �� ��"); 

        if (bindCheck == 0) bindCheck++;
        else if (bindCheck == 1) bindCheck++;

        bossRigidBody.velocity = Vector2.zero;

        SetAnimationTrigger("RunBindMotion");
        SetBossGodMode();                       //������ ���� ���·� ���� 
        RunBind();
    }
    public void RunBind()
    {
        StartCoroutine(BindProcess());
    }
    IEnumerator BindProcess()
    {
        Managers.Sound.PlaySFXAudio("Final_Boss_SFX/21_Debuff_01");
        bindEye = CreateSimpleAnimation(finalBossBindEye, player.transform, 0, 3);        //������ ������ ����
        Destroy(bindEye, 2.0f);                         //2�ʵ��� �����ϰ� �ٷ� �ı�
        for (int i = 0; i < keyListObject.Length; i++) 
        {
            qteGameObjectArray[i] = keyListObject[Random.Range(0, 6)];      //Ű���� ������ ������ ���� ����
            iconDestroy[i] = CreateSimpleAnimation(qteGameObjectArray[i], player.transform, x, 4);    //���� ����� �������� ȭ�鿡 �����ϰ� ���ÿ� ������ ������ ����
            x += 2.5f;      //�� �� ���� �� x��ǥ�� 2.5f��ŭ ����. (���������� �Űܰ���)
            switch (qteGameObjectArray[i].name)         //���ӿ�����Ʈ �迭�� �������� ����� �������� CHAR�� �迭�� �Ľ��ϴ� ����ġ��
            {
                case "BossIconA":
                    qteCharArray[i] = 'A'; break;
                case "BossIconS":
                    qteCharArray[i] = 'S'; break;
                case "BossIconD":
                    qteCharArray[i] = 'D'; break;
                case "BossIconZ":
                    qteCharArray[i] = 'Z'; break;
                case "BossIconX":
                    qteCharArray[i] = 'X'; break;
                case "BossIconC":
                    qteCharArray[i] = 'C'; break;
            }
        }
        
        yield return new WaitForSeconds(9.0f);  //9�ʵ��� �÷��̾��� Ű���� �Է��� ��ٸ�. 
        for (int m = 0; m < 6; m++)
        {
            inputList.Add(Managers.StageManager.Player.PlayerController.delevList[m]);
        }
        if (inputList.Equals(null))        //�÷��̾ �ƹ��͵� �Է����� �ʾҴٸ� ����
        {
            print("�÷��̾ �ƹ��͵� �Է����� �ʾ���");
            Managers.Sound.PlaySFXAudio("Final_Boss_SFX/wrong-answer-126515");
            qteCheck = false;
        }else if(inputList.Count != qteCharArray.Length)
        {
            print("�Էµ� ����Ʈ�� ���̰� �ٸ�");
            Managers.Sound.PlaySFXAudio("Final_Boss_SFX/wrong-answer-126515");
            qteCheck = false; 
        }
        else { 
            for (int num = 0; num < 6; num++)   //Ű���� ������ ������ ������ŭ �ݺ�
            {
                if (inputList[num].Equals(null))       //�˻� ���� �Է��� ���� �ʾҴٸ� ���� 
                {
                    print("�˻� ���� �Է��� ���� �ʾ���");
                    Managers.Sound.PlaySFXAudio("Final_Boss_SFX/wrong-answer-126515");
                    qteCheck = false;
                    break;
                }
                else if (qteCharArray[num] != inputList[num])    //���� ���� ������ �迭�� �Է¹��� ���� �ٸ��ٸ� ����
                {
                    print("���� ������ �迭�� �Է¹��� ���� �ٸ�");
                    Managers.Sound.PlaySFXAudio("Final_Boss_SFX/wrong-answer-126515");
                    qteCheck = false;
                    break;
                }
  
                qteCheck = true;                            //�� ���ǵ��� ����ϸ�, ����! 
            }
        }
        if (qteCheck == false)          //6�ʰ� ������ qte�� �����ߴٸ� �ӹ��� ��ӵ�
        {
            print("������"+inputList);
            
            Managers.Sound.PlaySFXAudio("Final_Boss_SFX/metal-chains");
            bindVineFail = CreateSimpleAnimation(finalBossBindVineFail, player.transform, 0, 1f);
            StartCoroutine(BindFalseProcess());
        }
        else if (qteCheck == true)     //6�ʰ� ������ qte�� ���������� ���� �ִϸ��̼��� ����ϰ� �ӹ��� Ǯ����.
        {

            Managers.Sound.PlaySFXAudio("Final_Boss_SFX/glass-breaking");
            bindVineSucess = CreateSimpleAnimation(finalBossBindVineSucess, player.transform, 0, 1f);

            Managers.StageManager.Player.PlayerController.isAttackalble = true;
            Managers.StageManager.Player.PlayerController.bossDebuff = false;
            
        }
        for (int k = 0; k < keyListObject.Length; k++)  //������ Ű���� ������ ������ ���� 
        {
            Destroy(iconDestroy[k]);
            print(iconDestroy[k]);
        }
        if (bindVineSucess != null) Destroy(bindVineSucess, 1.0f);      //�����ߴ� ������ �ı�
        if (bindVineFail != null) Destroy(bindVineFail, 5.0f);          //�����ߴ� ������ �ı�
        SetBossGodMode();                               //������ ������ ������
        PatternReset();                                 //���Ͽ� ����� �������� �ʱ�ȭ

        inputList.Clear();
        Managers.StageManager.Player.PlayerController.delevList.Clear();
    }
    IEnumerator BindFalseProcess()
    {
        yield return new WaitForSeconds(3.0f);

        Managers.StageManager.Player.PlayerController.isAttackalble = true;
        Managers.StageManager.Player.PlayerController.bossDebuff = false;
    }

    #endregion


    #region ��Ÿ �Լ���
    /// <summary>
    /// �Ű������� �޴� ��Ʈ���� ���� ���� �̸� ������ �ִϸ��̼��� �����Ű�� �Լ�
    /// </summary>
    /// <param name="trigger"></param>
    public void SetAnimationTrigger(string trigger)
    {
        bossAnimator.SetTrigger(trigger);
        //EnemyAnimator.SetTrigger(trigger);
    }
    /// <summary>
    /// ���� ���� �� ������ ������ ����� ���� ���� �Լ� true == ���� 
    /// �׻� �����Ǿ��ִ� ���� �ݴ�� �������. default�� false�� ���¿��� ����.
    /// </summary>
    private void SetBossGodMode(){ patternCheck = patternCheck != true; }//true�� ������ false��, false��� true�� �ٲ��ش�.
    /// <summary>
    /// ���� �������� ������ ������Ʈ�� ����üũ Ÿ�̸Ӹ� �ʱ�ȭ �����ִ� �Լ�
    /// </summary>
    public void PatternReset()
    {
        darkHealCheckTimer = 0;
        qteCheck = true;
        bossFSM.bossState = Define.BossState.MOVE_STATE;
    }
    /// <summary>
    /// �ܼ��� �����Ű�⸸ �ϸ� �Ǵ� �������� ������ �� ����ϴ� �Լ�.
    /// �ݶ��̴� ���� �ʿ��� �������� ��� �Ұ�. 
    /// </summary>
    /// <param name="runAnim">  �����ų ������ �ִϸ��̼� </param>        
    /// <param name="target"> ���� ������ �� ������Ʈ. ���� ���� this.gameObject�� ����ϸ� ��. </param>          
    /// <param name="distanceX"> �ִϸ��̼��� ����� x��ǥ </param>      
    /// <param name="distanceY"> �ִϸ��̼��� ����� y��ǥ </param>      
    /// <returns></returns>
    private GameObject CreateSimpleAnimation(GameObject runAnim, Transform target, float distanceX, float distanceY)
    {
        GameObject projectile = Instantiate(runAnim, new Vector2(target.transform.position.x + distanceX, 
                                                                 target.transform.position.y + distanceY), Quaternion.identity);
        projectile.SetActive(true);
        return projectile;      //������ ������Ʈ ���� ��������. 
    }
    /// <summary>
    ///  ����, ����ü�� ��������Ʈ �̹����� �÷��̾ �ִ� �������� �˸°� �ٲ��ִ� �Լ�
    /// </summary>
    /// <param name="target"></param>  
    private void SwitchSpriteImageDir(Transform target)
    {
        if (dir.x > 0) {target.localScale = new Vector2(scaleX, transform.localScale.y);}
        else{target.transform.localScale = new Vector2(-scaleX, transform.localScale.y);}
        //EnemyRigidbody.transform.localScale = new Vector2(scaleX, transform.localScale.y);
        //EnemyRigidbody.transform.localScale = new Vector2(-scaleX, transform.localScale.y);
    }
    #endregion
}

