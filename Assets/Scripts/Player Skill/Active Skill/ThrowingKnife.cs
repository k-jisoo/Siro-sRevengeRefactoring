using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ N�� ��ô
/// </summary>
public class ThrowingKnife : ActiveSkill
{
    [SerializeField] GameObject knifeObject;
    private readonly string[] knifeSFX = { "Player/Active Skill/Throwing_Knife_2", "Player/Active Skill/Throwing_Knife_2" };

    #region ��ų �⺻ ���� ������
    /// <summary>
    /// ��ų ������
    /// </summary>
    private int skillDamgae = 13;
    /// <summary>
    /// ǥâ ��ȯ ����
    /// </summary>
    private int skillProjectileCount = 2; 
    /// <summary>
    /// ǥâ ��ô�ӵ�
    /// </summary>
    private float skillProjectileSpeed = 15f;
    /// <summary>
    /// ǥâ ��ȯ���� (��� ��)
    /// </summary>
    private readonly WaitForSeconds SkillAttackDelay = new WaitForSeconds(0.25f);
    #endregion

    public override void OnActive()
    {
        if(currentSkillState == Define.CurrentSkillState.ACTIVE)
        {
            currentSkillState = Define.CurrentSkillState.COOL_TIME;
            ThrowingKnifeSkillAttack();
        }
        else
        {
            return;
        }
    }
    public override void Upgrade()
    {
        skillDamgae += 2;
        SkillCoolTime -= 0.5f;
        skillProjectileCount += 2;
        skillProjectileSpeed += 2f;
    }

    private void ThrowingKnifeSkillAttack()
    {
        StartCoroutine(ThrowingKnifeSkillProcess());
    }

     private IEnumerator ThrowingKnifeSkillProcess()
    {
        for (int projectiletCount = 0; projectiletCount < skillProjectileCount; projectiletCount++)
        {
            CreateProjectile();
            yield return SkillAttackDelay;
        }
        OnCoolTime();
    }


    private void CreateProjectile()
    {
        GameObject projectile = MemoryPoolManager.GetInstance().OutputGameObject(knifeObject,
                                                                                 "Player_Skill/"+knifeObject.name,
                                                                                 (transform.position + new Vector3(playerObject.PlayerController.LastDirection.x, playerObject.PlayerController.LastDirection.y, 0f)) + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)),
                                                                                 Quaternion.Euler(0, 0, Mathf.Atan2(-playerObject.PlayerController.LastDirection.y, -playerObject.PlayerController.LastDirection.x) * Mathf.Rad2Deg));

        projectile.GetComponent<Projectile>().ProjectileInit(Define.StringTag.Enemy, 
                                                             playerObject.PlayerController.LastDirection, 
                                                             skillDamgae, 
                                                             skillProjectileSpeed);
        projectile.SetActive(true);

        Managers.Sound.PlaySFXAudio(knifeSFX[Random.Range(0, knifeSFX.Length)], null, 0.5f);
    }
}
