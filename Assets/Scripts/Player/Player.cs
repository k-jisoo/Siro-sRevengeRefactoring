using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : LivingEntity
{
    /* ���� */

    #region �÷��̾� ���ó�� ����
    private bool isDead = false;
    public bool IsDead { get { return isDead; } }
    #endregion

    #region �÷��̾� �̹��� ����
    [SerializeField] Sprite[] attackSprites;
    #endregion

    #region �÷��̾� ��Ƽ���� ����
    [SerializeField] Material playerHitEffectMaterial;     // �ǰ� �� ��Ƽ����
    [SerializeField] Material orignalPlayerMaterial;       // �÷��̾� ���� ��Ƽ����
    private SpriteRenderer spriteRenderer;                 // SpriteRenderer ������Ʈ
    private WaitForSecondsRealtime seconds = new WaitForSecondsRealtime(0.25f);  // ��Ƽ���� ���� ������
    private WaitForSecondsRealtime spriteSeconds = new WaitForSecondsRealtime(1f); // ��������Ʈ ���� ������
    #endregion

    #region �÷��̾� ��Ʈ�ѷ� ����
    private PlayerController_ playerController;
    public PlayerController_ PlayerController { get { return playerController; } }
    #endregion

    #region �÷��̾� ��ȭ ����
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

    #region �÷��̾� ��ų �̺�Ʈ ����
    /// <summary>
    /// �ǰ� ���� �� �̺�Ʈ
    /// </summary>
    public event UnityAction HitEvent;
    /// <summary>
    /// ActiveSkill �ۿ�� �̺�Ʈ
    /// </summary>
    public UnityAction OnActiveSkillEvent;
    /// <summary>
    /// ���� ��ü �̺�Ʈ
    /// </summary>
    public UnityAction DisableBuffEvent;
    #endregion

    #region �÷��̾� ��ų Ȱ��ȭ ����
    public Dictionary<int, ActiveSkill> playerActiveSkills = new Dictionary<int, ActiveSkill>();
    public Dictionary<int, PassiveSkill> playerPassiveSkills = new Dictionary<int, PassiveSkill>();
    #endregion

    #region �������� ����ϴ� ��ų���� ����
    public Dictionary<string, PlayerSkill> skillList = new Dictionary<string, PlayerSkill>();

    // �÷��̾� ��Ƽ�� ��ų ����
    private int activeSkillSlot_Index = 0;
    public int ActiveSkillSlot_Index
    {
        get { return activeSkillSlot_Index; }
        set { activeSkillSlot_Index = value; }
    }
    #endregion

    #region �÷��̾� ȿ����
    private readonly string[] PLAYER_HIT_SFX = { "Player/PlayerHitSFX_1", "Player/PlayerHitSFX_2", "Player/PlayerHitSFX_3" };
    private readonly string PLAYER_DEAD_SFX = "Player/PlayerDeadSFX";
    #endregion

    /* �Լ� */

    #region �÷��̾� �ʱ� ���� 
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        PlayerInit();
        Managers.UI.InitUI();

    }

    /// <summary>
    /// PlayerController �� Player �ʱ� ���� �ʱ�ȭ 
    /// </summary>
    private void PlayerInit()
    {
        BasicStatInit();
        playerController = GetComponent<PlayerController_>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController.PlayerControllerInit(this);
    }
    #endregion

    #region �÷��̾� �ǰ� ��
    public override sealed void TakeDamage(int newDamage)
    {
        if (isDead) return;
        base.TakeDamage(newDamage);

        HitEvent?.Invoke(); // �ǰ� �� ���õ� �нú� ����� ȣ����
        StartCoroutine(SwitchMaterial()); // �ǰ� �� �÷��̾� ���� ���� �ڷ�ƾ


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
    /// �÷��̾� ���� ���� �ڷ�ƾ
    /// </summary>
    private IEnumerator SwitchMaterial()
    {
        spriteRenderer.material = playerHitEffectMaterial;
        yield return seconds;
        spriteRenderer.material = orignalPlayerMaterial;
    }
    #endregion

    #region �÷��̾� ��� ó��
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

    #region �÷��̾� �̹��� ������ ���� �Լ�

    /// <summary>
    /// ���� ��ų�̿�� �̹����� ����
    /// </summary>
    /// <param name="dir">�÷��̾� ����</param>
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