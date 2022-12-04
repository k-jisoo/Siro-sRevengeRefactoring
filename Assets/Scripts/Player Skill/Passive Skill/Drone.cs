using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Drone : PassiveSkill
{
    #region 드론 연출 효과 데이터
    private LineRenderer droneBulletLine;
    private Light2D droneMuzzleFlash;


    private Coroutine droneAttackCheck;
    private Coroutine droneRotateCheck;
    private bool isDroneStop = false;

    private readonly string[] droneSFX = { "Player/Passive Skill/Drone_1", "Player/Passive Skill/Drone_2" };
    private float lastTime = 0f;        // 드론 마지막 사격 시간
    private readonly float DRONE_MOVE_SPEED = 50f;              // 드론 이동 속도 (상수 값)
    private readonly float DRONE_BULLET_LINE_DURATION = 0.1f;   // 드론 공격 이펙트 지속 시간(상수 값)
    private readonly float CIRCLE_R = 3f;                       //반지름(상수 값)
    private float deg;                                          //각도
    private WaitForSeconds droneBulletLineDuration;    // 드론 공격 이펙트 지속시간 (상수 값)
    #endregion

    #region 스킬 기본 스텟 데이터
    /// <summary>
    /// 스킬 데미지
    /// </summary>
    private int skillDamage = 5;
    /// <summary>
    /// 스킬 공격 범위
    /// </summary>
    private float skillRange = 10f;
    /// <summary>
    /// 스킬 공격 대상 수
    /// </summary>
    private int skillTargetCount = 5;
    /// <summary>
    /// 스킬 공격 딜레이
    /// </summary>
    private float skillAttackDelay = 1f;     
    /// <summary>
    /// 스킬 공격 딜레이 코루틴
    /// </summary>
    private WaitForSeconds skillAttackDelayTimeSec;
    private float SkillAttackDelay
    {
        set
        {
            if(skillAttackDelay != value)
            {
                 skillAttackDelayTimeSec = new WaitForSeconds(value);
            }
            skillAttackDelay = value;
        }
    }
    #endregion

    private void Start()
    {
        DroneInit();
    }

    private void Update()  // 드론 사격 여부 확인
    {
        if (currentSkillState == Define.CurrentSkillState.ACTIVE)
        {
            currentSkillState = DroneSkillAttack();
        }
    }

    private void DroneInit()
    {
        droneBulletLineDuration = new WaitForSeconds(DRONE_BULLET_LINE_DURATION); // 캐싱
        skillAttackDelayTimeSec = new WaitForSeconds(skillAttackDelay);             // 캐싱

        droneBulletLine = GetComponent<LineRenderer>();
        droneMuzzleFlash = GetComponent<Light2D>();
        droneBulletLine.enabled = false; // 라인렌더러 컴포넌트 비활성화 
    }

    public override void OnActive()
    {
        DroneSkillActive();
    }

    public override void Upgrade()
    {
        skillDamage += 3;
        SkillCoolTime -= 1f;
        skillTargetCount += 3;
        SkillAttackDelay = (skillAttackDelay - 0.1f);
        skillRange += 2;
    }

    private void DroneSkillActive() // 드론 이동 여부 확인
    {
        if (droneAttackCheck == null && droneRotateCheck == null)
        {
            isDroneStop = false;
            droneRotateCheck = StartCoroutine(DroneAroundRotate(playerObject.transform));
        }
    }

    private Define.CurrentSkillState DroneSkillAttack()
    {
        Collider2D[] enemyCollider = Physics2D.OverlapCircleAll(playerObject.transform.position, skillRange, enemyLayer); // 플레이어 기준 10범위에 적을 탐지

        if (enemyCollider.Length > 0) // 적 배열이 0보다 많으면
        {
            StartCoroutine(DroneSkillAttackProcess(enemyCollider));
            return Define.CurrentSkillState.COOL_TIME;
        }
        return Define.CurrentSkillState.ACTIVE;
    }

    private IEnumerator DroneSkillAttackProcess(Collider2D[] enemyColliders)
    {
        int targetCount = enemyColliders.Length > skillTargetCount ? skillTargetCount : enemyColliders.Length;

        for (int enemyCount=0; enemyCount < targetCount; enemyCount++)
        {
            if (enemyColliders[enemyCount] != null)
            {
                if (playerObject.PlayerController.isAttackalble == false) StopAllCoroutines();

                Managers.Sound.PlaySFXAudio(droneSFX[0]);

                Enemy enemy = enemyColliders[enemyCount].GetComponent<Enemy>();
                StartCoroutine(DroneBulletEffect(enemy.transform.position));
                enemy.TakeDamage(skillDamage);

                Managers.Sound.PlaySFXAudio(droneSFX[1]);
                lastTime += skillAttackDelay;   // 공격 쿨타임에 타켓변경 시간까지 추가
                yield return skillAttackDelayTimeSec;
            }
        }
        droneAttackCheck = null;
        OnCoolTime();
    }



    #region 드론 연출 효과

    /// <summary>
    /// 드론 회전 코루틴
    /// </summary>
    /// <param name="target">회전활 기준점이 될 오브젝트</param>
    /// <returns></returns>
    private IEnumerator DroneAroundRotate(Transform target)
    {
        while (!isDroneStop)
        {
            deg += Time.deltaTime * DRONE_MOVE_SPEED;
            if (deg < 360)
            {
                var rad = Mathf.Deg2Rad * (deg);
                var x = CIRCLE_R * Mathf.Sin(rad);
                var y = CIRCLE_R * Mathf.Cos(rad);
                transform.position = target.position + new Vector3(x, y);
                //  transform.rotation = Quaternion.Euler(0, 0, deg); //가운데를 바라보게 각도 조절
            }
            else
            {
                deg = 0;
            }
            yield return null;
        }

        droneRotateCheck = null;
    }

    /// <summary>
    /// 드론 공격 이펙트
    /// </summary>
    /// <param name="enemyPosition">공격 대상 위치</param>
    /// <returns>droneBulletLineDuration 만큼 기다린 후 라인 렌더러 제거</returns>
    private IEnumerator DroneBulletEffect(Vector2 enemyPosition)
    {
        droneBulletLine.SetPosition(0, transform.position); // 라인 렌더러 시작 위치
        droneBulletLine.SetPosition(1, enemyPosition);      // 라인 렌더러 도착 위치

        droneMuzzleFlash.enabled = true; // 2D Light 컴포넌트 활성화
        droneBulletLine.enabled = true;  // 라인 렌더러 컴포넌트 활성화

        yield return droneBulletLineDuration;

        droneBulletLine.enabled = false; // 라인 렌더러 컴포넌트 비활성화
        droneMuzzleFlash.enabled = false; // 2D Light 컴포넌트 비활성화
    }

    #endregion
}
