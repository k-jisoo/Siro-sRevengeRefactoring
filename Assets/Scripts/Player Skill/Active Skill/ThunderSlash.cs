using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class ThunderSlash : ActiveSkill
{
    private TrailRenderer thunderclapEffect;
    [SerializeField] GameObject thunderSlashLockOnEffect;
    [SerializeField] GameObject thunderSlashHitEffetc;
    [SerializeField] VolumeProfile thunderClapProfile;

    #region 스킬 기본 스텟 데이터
    /// <summary>
    /// 스킬 데미지
    /// </summary>
    private int skillDamage = 25;
    /// <summary>
    /// 스킬 타겟 수
    /// </summary>
    private int skillTotalTarget = 2;
    #endregion

    private void Start()
    {
        ThunderSlashInit();
    }

    private void ThunderSlashInit()
    {
        thunderclapEffect = GetComponentInChildren<TrailRenderer>();
        thunderclapEffect.enabled = false;
    }

    public override void OnActive()
    {
        if (currentSkillState == Define.CurrentSkillState.ACTIVE)
        {
            currentSkillState = ThunderSlashSkillAttack();
        }
        else
        {
            return;
        }
    }

    public override void Upgrade()
    {
        skillDamage += 10;
        SkillCoolTime -= 2;
        skillTotalTarget += 3;
    }

    private Define.CurrentSkillState ThunderSlashSkillAttack()
    {
        Collider2D[] enemyCollider = Physics2D.OverlapCircleAll(playerObject.transform.position, 10f, enemyLayer);

        if (enemyCollider.Length > 0)
        {
            playerObject.PlayerController.isAttackalble = false; // 침묵 효과
            playerObject.PlayerController.isMoveable = false; // 이동제어


            OnSkillEffect();
            ThunderSlashEffect(); // 번개 효과 이펙트 (Trail Renderer) 활성화
            Time.timeScale = 0.1f;
            StartCoroutine(ThunderSlashSkillAttackSkillProcess(enemyCollider));
            return Define.CurrentSkillState.COOL_TIME;
        }
        return Define.CurrentSkillState.ACTIVE;
    }

    IEnumerator ThunderSlashSkillAttackSkillProcess(Collider2D[] enemyColliders)
    {
        GameObject[] lockOnEffect = new GameObject[skillTotalTarget];
        int targetCount = enemyColliders.Length > skillTotalTarget ? skillTotalTarget : enemyColliders.Length;


        for(int enemyCount = 0; enemyCount < targetCount; enemyCount++)
        {
            if (enemyColliders[enemyCount] != null)
            {
                lockOnEffect[enemyCount] = Instantiate(thunderSlashLockOnEffect, enemyColliders[enemyCount].transform.position, Quaternion.identity);
                Managers.Sound.PlaySFXAudio("Player/Active Skill/ThunderSlash_1");
                yield return new WaitForSecondsRealtime(0.5f);   // 캐싱 하자
            }
        }

        for (int enemyCount = 0; enemyCount < targetCount; enemyCount++)
        {
            playerObject.SwitchPlayerSprite((playerObject.transform.position - enemyColliders[enemyCount].transform.position).normalized, true);
            playerObject.transform.position = enemyColliders[enemyCount].transform.position;


            enemyColliders[enemyCount].GetComponent<Enemy>().TakeDamage(skillDamage);
            Instantiate(thunderSlashHitEffetc, enemyColliders[enemyCount].transform.position, Quaternion.identity);

             yield return new WaitForSecondsRealtime(0.5f);  // 캐싱 하자
            Destroy(lockOnEffect[enemyCount]);
        }
        OffSkillEffect();
        Time.timeScale = 1.0f;
     
        playerObject.PlayerController.isAttackalble = true; // 침묵 효과
        playerObject.PlayerController.isMoveable = true; // 이동제어

        Invoke(nameof(ThunderSlashEffect), 1f); // 번개 효과 이펙트 (Trail Renderer) 비활성화
        OnCoolTime(); // 스킬 쿨타임 진행
    }

    private void OnSkillEffect()
    {
        Managers.SkillEffectVolume.ChagnePostProcessProfile(thunderClapProfile);
    }

    private void OffSkillEffect()
    {
        Managers.SkillEffectVolume.ChagnePostProcessProfile(null);
    }

    private void ThunderSlashEffect()
    {
        thunderclapEffect.enabled = !thunderclapEffect.enabled;
    }

}
