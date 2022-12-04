using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;


public class PlayerController_ : MonoBehaviour
{
    

    #region 이동 관련 변수 선언부
    Vector3 moveDirection;                  //이동방향
    Vector2 lastDirection;                  //마지막 이동방향

    float playerTimeScale = 1.0f;    // 플레이어 시간정지 영향을 받는 속도
    public float PlayerTimeScale
    {
        set
        {
            playerTimeScale = value;
            customDeltaTime = playerTimeScale * Time.deltaTime;
        }
    } // 플레이어 시간정지 영향을 받는 속도
    float customDeltaTime;           // 플레이어 시간정지 영향을 받는 속도
    public Vector2 LastDirection { get { return lastDirection; } }
    #endregion

    #region 애니메이션 관련 변수 선언부
    Animator anim;                          
    public Animator Anim
    {
        get { return anim; }
    }
    BoxCollider2D boxCol;                   //콜라이더의 크기를 애니메이션에 맞게 조절하기 위해 사용
    #endregion

    #region 상태 제어 변수 선언부
    public bool isMoveable = true;             //기본 공격 때 움직임을 제한하기 위한 변수.
    public bool isAttackalble = true;   //스킬 사용 중 혹은 보스 몬스터에게 침묵이 걸렸을 때 스킬 사용을 제한하기 위한 변수.
    public bool bossDebuff = false;     // 보스에 대한 이동제어 제한
    #endregion

    #region 플레이어 정보 변수 선언부
    Player player;
    #endregion

    #region 보스 입력 이벤트 변수
    public List<char> delevList = new List<char>();
    #endregion

    #region 기본 공격 효과음 경로
    private readonly string[] defaullAttackSFX = { "Player/Default Attack/Player Default Attack_1",
                                                    "Player/Default Attack/Player Default Attack_2",
                                                    "Player/Default Attack/Player Default Attack_3" };
    #endregion

    #region 유니티 함수
    public void PlayerControllerInit(Player player)
    {
        this.player = player;
        anim = GetComponent<Animator>();
        boxCol = GetComponentInChildren<BoxCollider2D>();
    }

    private void Update()
    {
        if (isMoveable == true && !bossDebuff)              //기본 공격시 이동을 막기 위함.
        {
            Move();
        }
    }
    #endregion

    #region 이동 구현부
    public void OnMove(InputValue value)    //new input system 사용시 키 입력 받는 부분. 방향 키 입력을 받으면 OnMove()가 실행된다.
    {
        Vector2 input = value.Get<Vector2>();
        moveDirection = new Vector3(input.x, input.y, 0f);
    }

    private void Move()                     //Move 기능 구현부.
    {
        bool hasControl = (moveDirection != Vector3.zero);
        if (hasControl == true)
        {
    
            anim.SetBool("isMove", true);
            anim.SetFloat("X", moveDirection.x);
            anim.SetFloat("Y", moveDirection.y);

            transform.position += moveDirection * player.MoveSpeed * Time.unscaledDeltaTime;

            lastDirection = moveDirection;  //마지막 보고있는 방향 저장
        }
        else
        {
            anim.SetBool("isMove", false);
        }
    }
    #endregion

    #region 공격부
    /// <summary>
    /// 스킬이 없거나 쿨타임일땐 return 구문 필요.
    /// </summary>
    /// 
    void OnSkill1()
    {
        if (isAttackalble == true && Managers.StageManager.Player.playerActiveSkills.ContainsKey(0))
             player.playerActiveSkills[0].OnActive();
    }
    void OnSkill2()
    {
        //2번째 스킬 사용

        if (isAttackalble == true && Managers.StageManager.Player.playerActiveSkills.ContainsKey(1))
            player.playerActiveSkills[1].OnActive();
    }
    void OnSkill3()
    {
        //3번째 스킬 사용
        if (isAttackalble == true && Managers.StageManager.Player.playerActiveSkills.ContainsKey(2))
            player.playerActiveSkills[2].OnActive();
    }
    void OnSkill4()
    {
        //4번째 스킬 사용
        if (isAttackalble == true && Managers.StageManager.Player.playerActiveSkills.ContainsKey(3))
            player.playerActiveSkills[3].OnActive();
    }
    void OnSkill5()
    {
        //5번째 스킬 사용
        if (isAttackalble == true && Managers.StageManager.Player.playerActiveSkills.ContainsKey(4))
            player.playerActiveSkills[4].OnActive();
    }
    void OnAttack()
    {
        if (isAttackalble == true)
        {
            //기본공격 사용
            isMoveable = false;         //공격을 시작했을 때 움직임을 제한.
            anim.SetTrigger("BasicAttack");
        }
    }
    #endregion

    #region 보스 입력 이벤트 함수
    void OnNodeA() { if (bossDebuff) { delevList.Add('A'); } }
    void OnNodeS() { if (bossDebuff) { delevList.Add('S'); } }
    void OnNodeD() { if (bossDebuff) { delevList.Add('D'); } }
    void OnNodeZ() { if (bossDebuff) { delevList.Add('Z'); } }
    void OnNodeX() { if (bossDebuff) { delevList.Add('X'); } }
    void OnNodeC() { if (bossDebuff) { delevList.Add('C'); } }
    #endregion 보스 입력 이벤트 함수

    #region 애니메이션 이벤트 함수
    void SetIsMoveableTrue()        //공격이 끝났을 때 움직이게 할 수 있도록 하는 애니메이션 이벤트 함수.
        => isMoveable = true;
    void SetColliderEnabled()   // 플레이어가 기본 공격시 콜라이더를 켜는 애니메이션 이벤트 함수.
    {
        Managers.Sound.PlaySFXAudio(defaullAttackSFX[Random.Range(0, defaullAttackSFX.Length)]);
        boxCol.enabled = true;
    }
    void SetColliderDisabled()      //플레이어가 기본 공격시 콜라이더를 끄는 애니메이션 이벤트 함수.
    {
        boxCol.enabled = false;
        player.DisableBuffEvent?.Invoke();
    }
    #endregion
}
