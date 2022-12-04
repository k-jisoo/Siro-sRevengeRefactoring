using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Boss : Enemy
{
    

    #region 사용된 모든 상수
    readonly float BOSS_PROJECTILE_SKULL_SPEED = 10f;       //기본공격 투사체 속도
    readonly float BOSS_PATTERN_DARK_HEAL_COUNT = 30f;      //다크힐 패턴의 시작 쿨타임 통제 변수
    readonly int BOSS_TEMP_HP = 100;                        //다크힐 패턴 시 사용되는 임시체력.
    #endregion             

    #region 편리를 위한 모든 시리얼라이즈 필드 
    [SerializeField] [Range(0f, 50f)] float contactDistance;    //보스의 사정거리 초기값 10
    [SerializeField] GameObject finalBossSkull;             //기본공격 프리팹
    [SerializeField] GameObject finalBossRuinStk;           //루인스트라이크 패턴 프리팹
    [SerializeField] GameObject finalBossDarkHeal;          //다크힐 패턴 차징 프리팹
    [SerializeField] GameObject finalBossDarkHealFailed;    //다크힐 패턴 실패 프리팹
    [SerializeField] GameObject finalBossBindEye;           //바인드 패턴 시 플레이어 감시 프리팹
    [SerializeField] GameObject finalBossBindVineFail;      //바인드 패턴 실패 프리팹 
    [SerializeField] GameObject finalBossBindVineSucess;    //바인드 패턴 성공 프리팹
    [SerializeField] GameObject finalBossInputSoul;         //Seeker 소환 시 사용할 프리팹 
    [SerializeField] GameObject[] keyListObject;            //바인드 패턴 키 모양 출력 프리팹
    [SerializeField] Material originalMaterial;
    [SerializeField] Material hurtMaterial;
   // [SerializeField] GameObject player;         //플레이어 프리팹 
    [SerializeField] GameObject seekerPrefab;

    Player player;
    


    #endregion

    #region Destroy처리에 사용한 GameObject 변수
    GameObject darkHealA;
    GameObject darkHealB;
    GameObject bindEye;
    GameObject bindVineSucess;
    GameObject bindVineFail;
    GameObject skeletonSeekrer;
    #endregion

    #region Pattern1_DefaultAttack_Flying Skull 관련 전역변수



    #endregion

    #region Pattern2_Dark Heal 관련 전역변수
    private float darkHealCheckTimer = 0.0f;    //다크힐 임시체력 파괴시간을 통제하는 변수 
    private int darkHealTempHp;                 //다크힐 임시체력 상수값을 저장하는 변수
    #endregion

    #region Pattern3_Ruin Strike 관련 전역변수
    private int ruinStrikeQty;                  //루인스트라이크 갯수 조절 변수
    private int ruinStrikeCheck = 0;
    #endregion

    #region Pattern4_Summon Skeleton 관련 전역변수
    private int sumnSkeletonCheck = 0;
    #endregion

    #region Pattern5_Bind 관련 전역변수
    private int bindCheck = 0;
    GameObject[] iconDestroy = new GameObject[6];           //키보드 프리팹을 한번에 삭제하기 위한 변수선언 
    GameObject[] qteGameObjectArray = new GameObject[6];    //키보드 프리팹 오브젝트를 담기 위한 선언
    char[] qteCharArray = new char[6];                      //Object배열 형식을 Char형식응로 변환하기 위해 선언
    List<char> inputList = new List<char>();                //플레이어의 입력 값을 저장하기 위해 선언
    private float x = -8f;                                  //키보드 프리팹 생성 X좌표 기준 값 변수. 가변적임
    private bool qteCheck = true;                          //QTE를 플레이어가 성공한다면 true, 실패면 false
    #endregion

    #region 기타 전역변수
    BossFSM bossFSM;
    Rigidbody2D bossRigidBody;
    Animator bossAnimator;
    SpriteRenderer bossSpriteRenderer;
    Transform targetIsPlayer;                   //플레이어의 위치정보 저장
    private float scaleX;
    private Vector2 dir;
    private float bossHpPercentage=100f;            //보스의 체력을 퍼센티지로 나타내 저장하는 변수
    private float patternCheckTimer = 0.0f;     //일정시간이 지나고 특정 패턴을 실행시키고 싶을 때 사용
    private bool patternCheck = false;          // == isgod. true이면 무적임. 
    private bool isStart = false;
    #endregion

    [SerializeField] Spawner_SkeletonSeeker SkeletonSeekerSpawner;

    /// <summary>
    /// 클래스를 실행 시키기 전 필요한 정보를 모두 대입함
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
        
        bossFSM.bossState = Define.BossState.CASTING_STATE; //연출효과 대기
 
    }

    #region 보스 연출이 종료되면 Move 상태로 전환
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
        if (bossFSM != null) bossFSM.Update();                      //보스의 STATE값이 있을 때만 동작함. 
        if (bossFSM.bossState != Define.BossState.CASTING_STATE)    //보스가 패턴 중일 때는 패턴체크타이머를 증가하지 않음
        {
            patternCheckTimer += Time.deltaTime;
        }
        if (patternCheckTimer > BOSS_PATTERN_DARK_HEAL_COUNT)       //n초마다 다크힐 패턴 실행
        {
            bossFSM.bossState = Define.BossState.PATTERN_DARKHEAL_STATE;
            patternCheckTimer = 0;
        }
    }

    #region 보스 기본 스테이트, 패턴 시작 함수들
    /// <summary>
    /// 보스와 공격 사정거리에 따라 추적하거나 이동시킴
    /// </summary>
    public void Move()
    {
        //만약 보스와 플레이어의 거리차이가 미리설정한 사정거리보다 크다면 플레이어를 추적, 아니라면 공격 STATE를 실행
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
    /// 보스의 공격사정거리에 따라 플레이어를 공격하거나 MOVE_STATE를 실행시킴
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
    /// 보스의 기본공격 중 해골 투사체를 생성하는 함수
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
    /// 플레이어에게 데미지를 받을 때 처리되는 함수
    /// </summary>
    public override sealed void TakeDamage(int newDamage)
    {
        if (patternCheck)              //패턴 진행 중이라 무적 처리를 해야할 때
        {
            return;
        }
        if (bossFSM.runDarkHeal)       //다크힐 패턴 실행중일 때는 플레이어가 가하는 데미지가 임시체력 변수값을 차감 하도록.
        {
            darkHealTempHp -= newDamage;
            Hurt();
            return;
        }
        Hurt();
        base.TakeDamage(newDamage);
        Managers.UI.UpdateBossHpSlider(Hp, MaxHp);
        bossHpPercentage = (float)Hp / (float)MaxHp * 100;        //현재 체력 나누기 최대체력 곱하기 백
        print("데미지 계산 처리 후 보스 현재 체력"+ bossHpPercentage);
        //Managers.StageManager.IsBossAlive(Hp)
        //Managers.UI.UpdateBossHpSlider(Hp, MaxHp);

        RunPattern(bossHpPercentage);  //만약 대입된 보스의 체력이 패턴을 실행 시켜야 되는 체력이라면 그에 맞게 패턴을 실행.
        if(bossHpPercentage <= 0f) 
        {
            OnDead();
        }
    }
    /// <summary>
    /// 피격효과 재생 함수 
    /// </summary>
    public void Hurt()
    {
        StartCoroutine(SwitchMaterial());
    }
    /// <summary>
    /// 피격효과를 일정시간 유지시킴. (스프라이트를 흰색으로 만듬)
    /// </summary>
    IEnumerator SwitchMaterial()
    {
        Managers.Sound.PlaySFXAudio("Final_Boss_SFX/11_human_damage_3");
        bossSpriteRenderer.material = hurtMaterial;
        yield return new WaitForSeconds(0.1f);
        bossSpriteRenderer.material = originalMaterial;
    }
    /// <summary>
    /// 보스의 현재체력이 0이되면 실행시키는 함수
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
    /// 보스의 체력에 따라 해당하는 기능을 실행시키는 함수
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

    #region 다크 힐 패턴 관련 함수들
    /// <summary>
    /// 패턴 다크 힐 실행 함수
    /// 1분 30초마다 반복되고 8초동안 에너지를 모음
    /// 만약 데미지를 1,000 이상 받게 된다면 모으고 있던 에너지는 흩어짐(이후 플레이어 다시 재추적)
    /// 보스 최대 체력의 15%를 회복함 
    /// </summary>
    public void Pattern_DarkHeal()
    {
        bossFSM.bossState = Define.BossState.CASTING_STATE; //스테이트 변경
        bossRigidBody.velocity = Vector2.zero;              //보스 좌표 고정
        SetAnimationTrigger("RunDarkHealMotion");           //애니메이션 트리거 실행
        RunDarkHeal();                                      //패턴함수 실행
        Managers.Sound.PlaySFXAudio("Final_Boss_SFX/LostArkSkillSfx1");
    }
    public void RunDarkHeal(){  StartCoroutine(DarkHealProcess());  }
    IEnumerator DarkHealProcess()
    {
        darkHealA = CreateSimpleAnimation(finalBossDarkHeal, this.gameObject.transform, 0, 3);
        while (true)
        {

            if (darkHealCheckTimer >= 7.0f && darkHealTempHp > 0)          //시간 카운터가 8초가 지나고 1,000 데미지 이하로 받았을 때
            {
                Hp += MaxHp * 15 / 100;     //전체값의 15%만큼 증가
                Managers.UI.UpdateBossHpSlider(Hp, MaxHp);
                break;
            }
            else if (darkHealTempHp <= 0)     //시간 카운터가 8초가 지나지 않고 1,000데미지 이상으로 받았을 때
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

    #region 루인 스트라이크 패턴 관련 함수들
    /// <summary>
    /// 패턴 루인 스트라이크 실행 함수
    /// 보스의 체력이 40%, 25%, 5%가 되었을 때마다 실행 
    /// 들어오는 조건에 따라 4개,6,개,9개의 투사체가 랜덤한 위치에 생성됨
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
    /// 루인스트라이크 코루틴을 실행시키는 함수
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
            print(i + "번 발사");
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

    #region 시커스켈레톤 소환관련 함수들
    /// <summary>
    /// 패턴 스켈레톤 소환 실행 함수
    /// 보스의 체력이 50%가 되었을 때 시전되며 개체수는 미정. 
    /// 스켈레톤의 공격력은 20, 기본 근접 공격밖에 없음. 체력은 조금 높게 설정. 
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

    #region 바인드 패턴 관련 함수들
    /// <summary>
    /// 패턴 바인드 실행 함수
    /// 체력 70%, 15%가 되었을 때 실행됨. 
    /// 4초 동안 6개의 커맨드를 쳐야 됨. 
    /// Quick Time Event가 동시에 실행되며 이를 실패하면 5초 속박. 
    /// </summary>
    public void Pattern_Bind()
    {
        bossFSM.bossState = Define.BossState.CASTING_STATE;
        print("false해줌 ");
        Managers.StageManager.Player.PlayerController.delevList.Clear();
        Managers.StageManager.Player.PlayerController.isAttackalble = false;
        Managers.StageManager.Player.PlayerController.bossDebuff = true;    
        print("false 해주고 난 뒤"); 

        if (bindCheck == 0) bindCheck++;
        else if (bindCheck == 1) bindCheck++;

        bossRigidBody.velocity = Vector2.zero;

        SetAnimationTrigger("RunBindMotion");
        SetBossGodMode();                       //보스를 무적 상태로 만듬 
        RunBind();
    }
    public void RunBind()
    {
        StartCoroutine(BindProcess());
    }
    IEnumerator BindProcess()
    {
        Managers.Sound.PlaySFXAudio("Final_Boss_SFX/21_Debuff_01");
        bindEye = CreateSimpleAnimation(finalBossBindEye, player.transform, 0, 3);        //눈동자 프리팹 생성
        Destroy(bindEye, 2.0f);                         //2초동안 유지하고 바로 파괴
        for (int i = 0; i < keyListObject.Length; i++) 
        {
            qteGameObjectArray[i] = keyListObject[Random.Range(0, 6)];      //키보드 아이콘 프리팹 랜덤 저장
            iconDestroy[i] = CreateSimpleAnimation(qteGameObjectArray[i], player.transform, x, 4);    //랜덤 저장된 프리팹을 화면에 생성하고 동시에 생성된 정보를 저장
            x += 2.5f;      //한 번 생성 시 x좌표는 2.5f만큼 더함. (오른쪽으로 옮겨가짐)
            switch (qteGameObjectArray[i].name)         //게임오브젝트 배열에 랜덤으로 저장된 프리팹을 CHAR형 배열로 파싱하는 스위치문
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
        
        yield return new WaitForSeconds(9.0f);  //9초동안 플레이어의 키보드 입력을 기다림. 
        for (int m = 0; m < 6; m++)
        {
            inputList.Add(Managers.StageManager.Player.PlayerController.delevList[m]);
        }
        if (inputList.Equals(null))        //플레이어가 아무것도 입력하지 않았다면 실패
        {
            print("플레이어가 아무것도 입력하지 않았음");
            Managers.Sound.PlaySFXAudio("Final_Boss_SFX/wrong-answer-126515");
            qteCheck = false;
        }else if(inputList.Count != qteCharArray.Length)
        {
            print("입력된 리스트의 길이가 다름");
            Managers.Sound.PlaySFXAudio("Final_Boss_SFX/wrong-answer-126515");
            qteCheck = false; 
        }
        else { 
            for (int num = 0; num < 6; num++)   //키보드 아이콘 프리팹 갯수만큼 반복
            {
                if (inputList[num].Equals(null))       //검사 도중 입력이 되지 않았다면 실패 
                {
                    print("검사 도중 입력이 되지 않았음");
                    Managers.Sound.PlaySFXAudio("Final_Boss_SFX/wrong-answer-126515");
                    qteCheck = false;
                    break;
                }
                else if (qteCharArray[num] != inputList[num])    //만약 랜덤 생성된 배열과 입력받은 값이 다르다면 실패
                {
                    print("랜덤 생성된 배열과 입력받은 값이 다름");
                    Managers.Sound.PlaySFXAudio("Final_Boss_SFX/wrong-answer-126515");
                    qteCheck = false;
                    break;
                }
  
                qteCheck = true;                            //위 조건들을 통과하면, 성공! 
            }
        }
        if (qteCheck == false)          //6초가 지나고 qte에 실패했다면 속박이 계속됨
        {
            print("실패함"+inputList);
            
            Managers.Sound.PlaySFXAudio("Final_Boss_SFX/metal-chains");
            bindVineFail = CreateSimpleAnimation(finalBossBindVineFail, player.transform, 0, 1f);
            StartCoroutine(BindFalseProcess());
        }
        else if (qteCheck == true)     //6초가 지나고 qte에 성공했으면 성공 애니메이션을 재생하고 속박을 풀어줌.
        {

            Managers.Sound.PlaySFXAudio("Final_Boss_SFX/glass-breaking");
            bindVineSucess = CreateSimpleAnimation(finalBossBindVineSucess, player.transform, 0, 1f);

            Managers.StageManager.Player.PlayerController.isAttackalble = true;
            Managers.StageManager.Player.PlayerController.bossDebuff = false;
            
        }
        for (int k = 0; k < keyListObject.Length; k++)  //생성된 키보드 아이콘 프리팹 삭제 
        {
            Destroy(iconDestroy[k]);
            print(iconDestroy[k]);
        }
        if (bindVineSucess != null) Destroy(bindVineSucess, 1.0f);      //생성했던 프리팹 파괴
        if (bindVineFail != null) Destroy(bindVineFail, 5.0f);          //생성했던 프리팹 파괴
        SetBossGodMode();                               //보스의 무적을 해제함
        PatternReset();                                 //패턴에 사용한 변수값들 초기화

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


    #region 기타 함수들
    /// <summary>
    /// 매개변수로 받는 스트링의 값에 따라 미리 설정된 애니메이션을 실행시키는 함수
    /// </summary>
    /// <param name="trigger"></param>
    public void SetAnimationTrigger(string trigger)
    {
        bossAnimator.SetTrigger(trigger);
        //EnemyAnimator.SetTrigger(trigger);
    }
    /// <summary>
    /// 패턴 시작 시 보스를 무적을 만들기 위해 쓰는 함수 true == 무적 
    /// 항상 설정되어있는 값의 반대로 만들어줌. default는 false인 상태에서 실행.
    /// </summary>
    private void SetBossGodMode(){ patternCheck = patternCheck != true; }//true가 들어오면 false로, false라면 true로 바꿔준다.
    /// <summary>
    /// 패턴 마지막에 보스의 스테이트와 패턴체크 타이머를 초기화 시켜주는 함수
    /// </summary>
    public void PatternReset()
    {
        darkHealCheckTimer = 0;
        qteCheck = true;
        bossFSM.bossState = Define.BossState.MOVE_STATE;
    }
    /// <summary>
    /// 단순히 실행시키기만 하면 되는 프리팹을 실행할 때 사용하는 함수.
    /// 콜라이더 등이 필요한 프리팹은 사용 불가. 
    /// </summary>
    /// <param name="runAnim">  실행시킬 프리팹 애니메이션 </param>        
    /// <param name="target"> 실행 기준이 될 오브젝트. 보스 사용시 this.gameObject를 사용하면 됨. </param>          
    /// <param name="distanceX"> 애니메이션이 실행될 x좌표 </param>      
    /// <param name="distanceY"> 애니메이션이 실행될 y좌표 </param>      
    /// <returns></returns>
    private GameObject CreateSimpleAnimation(GameObject runAnim, Transform target, float distanceX, float distanceY)
    {
        GameObject projectile = Instantiate(runAnim, new Vector2(target.transform.position.x + distanceX, 
                                                                 target.transform.position.y + distanceY), Quaternion.identity);
        projectile.SetActive(true);
        return projectile;      //생성한 오브젝트 값을 리턴해줌. 
    }
    /// <summary>
    ///  보스, 투사체의 스프라이트 이미지를 플레이어가 있는 방향으로 알맞게 바꿔주는 함수
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

