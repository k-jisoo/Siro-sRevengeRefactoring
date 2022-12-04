using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WindDash : ActiveSkill
{
    private TrailRenderer windDlashEffect;
    private readonly string windDashSFX = "Player/Active Skill/Wind Dash";

    #region 스킬 기본 스텟 데이터
    /// <summary>
    /// 스킬 데미지
    /// </summary>
    private int skillDamgae = 6;
    /// <summary>
    /// 스킬 대쉬 최대길이
    /// </summary>
    private float skillDashDistacne = 5f;
    #endregion

    private void Start()
    {
        WindSlashInit();
    }

    private void WindSlashInit()
    {
        windDlashEffect = GetComponentInChildren<TrailRenderer>();
        windDlashEffect.enabled = true;
        windDlashEffect.enabled = false;
    }

    public override void OnActive()
    {
        if (currentSkillState == Define.CurrentSkillState.ACTIVE)
        {
            currentSkillState = Define.CurrentSkillState.COOL_TIME;
            WindSlashSkillAttack();
        }
        else
        {
            // TODO : UI에서 "아직 재사용 대기시간 입니다." 연출하기
            return;
        }
    }

    public override void Upgrade()
    {
        skillDamgae += 2;
        SkillCoolTime -= 1;
        skillDashDistacne += 1.5f;
        // TODO : 상점에서 업그레이드 방식이 정해지면 진행 하자 (09/28)
    }

    private void WindSlashSkillAttack()
    {

        playerObject.SwitchPlayerSprite(playerObject.PlayerController.LastDirection, false);
        playerObject.PlayerController.isAttackalble = false;
        playerObject.PlayerController.isMoveable = false;
        OnSkillEffect();  // 이펙트 활성화

        Vector3 firstPosition = playerObject.transform.position;
        float maxDistance = skillDashDistacne;


        RaycastHit2D hitObject = Physics2D.Raycast(transform.position, playerObject.PlayerController.LastDirection, maxDistance, wallLayer);

        if (hitObject)  // RayCast가 벽에 충돌했다는 의미
        {
            maxDistance = hitObject.distance-1f;     // 벽 충돌 위치까지만 Ray를 쏘기 위한 거리 측정
            playerObject.transform.Translate(playerObject.PlayerController.LastDirection.normalized * maxDistance); // 마지막으로 본 방향 + dashDistance 길이 만큼 이동
        }

        else
        {
            playerObject.transform.Translate(playerObject.PlayerController.LastDirection.normalized * maxDistance); // 마지막으로 본 방향 + dashDistance 길이 만큼 이동
        }

        Managers.Sound.PlaySFXAudio(windDashSFX, null, 0.5f, false);

        RaycastHit2D[] enemyObjects = Physics2D.RaycastAll(firstPosition, playerObject.PlayerController.LastDirection, maxDistance, enemyLayer);


        if (enemyObjects.Length > 0)
        {
            for (int enemyCount = 0; enemyCount < enemyObjects.Length; enemyCount++)
            {
                enemyObjects[enemyCount].transform.GetComponent<Enemy>().TakeDamage(skillDamgae);
            }
        }

        playerObject.PlayerController.isAttackalble = true;
        playerObject.PlayerController.isMoveable = true;

        Invoke(nameof(OnSkillEffect), 1.5f); //  이펙트 비활성화
        OnCoolTime();

    }

    private void OnSkillEffect()
    {
        windDlashEffect.enabled = !windDlashEffect.enabled;
    }
}