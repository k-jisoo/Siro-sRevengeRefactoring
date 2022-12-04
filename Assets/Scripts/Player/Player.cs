using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : LivingEntity
{
    /* 변수 */

    #region 플레이어 사망처리 변수
    private bool isDead = false;
    public bool IsDead { get { return isDead; } }
    #endregion

    #region 플레이어 이미지 변수
    [SerializeField] Sprite[] attackSprites;
    #endregion

    #region 플레이어 머티리얼 변수
    [SerializeField] Material playerHitEffectMaterial;     // 피격 시 머티리얼
    [SerializeField] Material orignalPlayerMaterial;       // 플레이어 원본 머티리얼
    private SpriteRenderer spriteRenderer;                 // SpriteRenderer 컴포넌트
    private WaitForSecondsRealtime seconds = new WaitForSecondsRealtime(0.25f);  // 머티리얼 변경 딜레이
    private WaitForSecondsRealtime spriteSeconds = new WaitForSecondsRealtime(1f); // 스프라이트 변경 딜레이
    #endregion

    #region 플레이어 컨트롤러 변수
    private PlayerController_ playerController;
    public PlayerController_ PlayerController { get { return playerController; } }
    #endregion

    #region 플레이어 재화 변수
    private int playerGold = 0;
    public int PlayerGold
    {
        get { return playerGold; }
        set
        {
            playerGold = value;
            Managers.UI.UpdateGoldText();
        }
    }
    #endregion

    #region 플레이어 스킬 이벤트 변수
    /// <summary>
    /// 피격 받을 시 이벤트
    /// </summary>
    public event UnityAction HitEvent;
    /// <summary>
    /// ActiveSkill 작용시 이벤트
    /// </summary>
    public UnityAction OnActiveSkillEvent;
    /// <summary>
    /// 버프 해체 이벤트
    /// </summary>
    public UnityAction DisableBuffEvent;
    #endregion

    #region 플레이어 스킬 활성화 변수
    public Dictionary<int, ActiveSkill> playerActiveSkills = new Dictionary<int, ActiveSkill>();
    public Dictionary<int, PassiveSkill> playerPassiveSkills = new Dictionary<int, PassiveSkill>();
    #endregion

    #region 상점에서 사용하는 스킬종류 변수
    public Dictionary<string, PlayerSkill> skillList = new Dictionary<string, PlayerSkill>();

    // 플레이어 액티브 스킬 슬롯
    private int activeSkillSlot_Index = 0;
    public int ActiveSkillSlot_Index
    {
        get { return activeSkillSlot_Index; }
        set { activeSkillSlot_Index = value; }
    }
    #endregion

    #region 플레이어 효과음
    private readonly string[] PLAYER_HIT_SFX = { "Player/PlayerHitSFX_1", "Player/PlayerHitSFX_2", "Player/PlayerHitSFX_3" };
    private readonly string PLAYER_DEAD_SFX = "Player/PlayerDeadSFX";
    #endregion

    /* 함수 */

    #region 플레이어 초기 설정 
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        PlayerInit();
        Managers.UI.InitUI();

    }

    /// <summary>
    /// PlayerController 랑 Player 초기 스텟 초기화 
    /// </summary>
    private void PlayerInit()
    {
        BasicStatInit();
        playerController = GetComponent<PlayerController_>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController.PlayerControllerInit(this);
    }
    #endregion

    #region 플레이어 피격 시
    public override sealed void TakeDamage(int newDamage)
    {
        if (isDead) return;
        base.TakeDamage(newDamage);

        HitEvent?.Invoke(); // 피격 시 관련된 패시브 기술만 호출함
        StartCoroutine(SwitchMaterial()); // 피격 시 플레이어 색상 변경 코루틴


        GameObject floatingText = MemoryPoolManager.GetInstance().OutputGameObject
            (Managers.Resource.GetPerfabGameObject("UI/DamageText")
            , "UI/DamageText"
            , new Vector3(transform.position.x, transform.position.y)
            , Quaternion.identity);

        floatingText.GetComponent<FloatingText>().DamageText = newDamage.ToString();
        floatingText.SetActive(true);
        Managers.Sound.PlaySFXAudio(PLAYER_HIT_SFX[Random.Range(0, PLAYER_HIT_SFX.Length)]);


        Managers.UI.UpdatePlayerHpSlider(Hp, MaxHp);
    }

    /// <summary>
    /// 플레이어 색상 변경 코루틴
    /// </summary>
    private IEnumerator SwitchMaterial()
    {
        spriteRenderer.material = playerHitEffectMaterial;
        yield return seconds;
        spriteRenderer.material = orignalPlayerMaterial;
    }
    #endregion

    #region 플레이어 사망 처리
    protected override void OnDead()
    {
        playerController.isAttackalble = false;
        playerController.isMoveable = false;
        isDead = true;
        Managers.Sound.PlaySFXAudio(PLAYER_DEAD_SFX);
        StopAllCoroutines();
        gameObject.layer = 0;
        playerController.Anim.SetTrigger("isDead");
    }

    private void PlayerDeadEvent()
    {
        SceneManager.LoadScene("Ending");
        gameObject.SetActive(false);
    }
    #endregion

    #region 플레이어 이미지 랜더러 변경 함수

    /// <summary>
    /// 돌진 스킬이용시 이미지를 변경
    /// </summary>
    /// <param name="dir">플레이어 방향</param>
    public void SwitchPlayerSprite(Vector2 dir, bool needDelay)
    {
        playerController.Anim.enabled = false;
        StartCoroutine(SwitchSprite(dir.normalized, needDelay));
    }

    private IEnumerator SwitchSprite(Vector2 dir, bool isDelay)
    {

        if (dir.y > 0) spriteRenderer.sprite = attackSprites[0];
        else if (dir.y < 0) spriteRenderer.sprite = attackSprites[1];
        else spriteRenderer.sprite = attackSprites[2];

        if (isDelay)
            yield return spriteSeconds;
        else
            yield return seconds;

        playerController.Anim.enabled = true;
    }
    #endregion
}