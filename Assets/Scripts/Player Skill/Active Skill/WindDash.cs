using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WindDash : ActiveSkill
{
    private TrailRenderer windDlashEffect;
    private readonly string windDashSFX = "Player/Active Skill/Wind Dash";

    #region ��ų �⺻ ���� ������
    /// <summary>
    /// ��ų ������
    /// </summary>
    private int skillDamgae = 6;
    /// <summary>
    /// ��ų �뽬 �ִ����
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
            // TODO : UI���� "���� ���� ���ð� �Դϴ�." �����ϱ�
            return;
        }
    }

    public override void Upgrade()
    {
        skillDamgae += 2;
        SkillCoolTime -= 1;
        skillDashDistacne += 1.5f;
        // TODO : �������� ���׷��̵� ����� �������� ���� ���� (09/28)
    }

    private void WindSlashSkillAttack()
    {

        playerObject.SwitchPlayerSprite(playerObject.PlayerController.LastDirection, false);
        playerObject.PlayerController.isAttackalble = false;
        playerObject.PlayerController.isMoveable = false;
        OnSkillEffect();  // ����Ʈ Ȱ��ȭ

        Vector3 firstPosition = playerObject.transform.position;
        float maxDistance = skillDashDistacne;


        RaycastHit2D hitObject = Physics2D.Raycast(transform.position, playerObject.PlayerController.LastDirection, maxDistance, wallLayer);

        if (hitObject)  // RayCast�� ���� �浹�ߴٴ� �ǹ�
        {
            maxDistance = hitObject.distance-1f;     // �� �浹 ��ġ������ Ray�� ��� ���� �Ÿ� ����
            playerObject.transform.Translate(playerObject.PlayerController.LastDirection.normalized * maxDistance); // ���������� �� ���� + dashDistance ���� ��ŭ �̵�
        }

        else
        {
            playerObject.transform.Translate(playerObject.PlayerController.LastDirection.normalized * maxDistance); // ���������� �� ���� + dashDistance ���� ��ŭ �̵�
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

        Invoke(nameof(OnSkillEffect), 1.5f); //  ����Ʈ ��Ȱ��ȭ
        OnCoolTime();

    }

    private void OnSkillEffect()
    {
        windDlashEffect.enabled = !windDlashEffect.enabled;
    }
}