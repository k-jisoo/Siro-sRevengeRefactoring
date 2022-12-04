using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Drone : PassiveSkill
{
    #region ��� ���� ȿ�� ������
    private LineRenderer droneBulletLine;
    private Light2D droneMuzzleFlash;


    private Coroutine droneAttackCheck;
    private Coroutine droneRotateCheck;
    private bool isDroneStop = false;

    private readonly string[] droneSFX = { "Player/Passive Skill/Drone_1", "Player/Passive Skill/Drone_2" };
    private float lastTime = 0f;        // ��� ������ ��� �ð�
    private readonly float DRONE_MOVE_SPEED = 50f;              // ��� �̵� �ӵ� (��� ��)
    private readonly float DRONE_BULLET_LINE_DURATION = 0.1f;   // ��� ���� ����Ʈ ���� �ð�(��� ��)
    private readonly float CIRCLE_R = 3f;                       //������(��� ��)
    private float deg;                                          //����
    private WaitForSeconds droneBulletLineDuration;    // ��� ���� ����Ʈ ���ӽð� (��� ��)
    #endregion

    #region ��ų �⺻ ���� ������
    /// <summary>
    /// ��ų ������
    /// </summary>
    private int skillDamage = 5;
    /// <summary>
    /// ��ų ���� ����
    /// </summary>
    private float skillRange = 10f;
    /// <summary>
    /// ��ų ���� ��� ��
    /// </summary>
    private int skillTargetCount = 5;
    /// <summary>
    /// ��ų ���� ������
    /// </summary>
    private float skillAttackDelay = 1f;     
    /// <summary>
    /// ��ų ���� ������ �ڷ�ƾ
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

    private void Update()  // ��� ��� ���� Ȯ��
    {
        if (currentSkillState == Define.CurrentSkillState.ACTIVE)
        {
            currentSkillState = DroneSkillAttack();
        }
    }

    private void DroneInit()
    {
        droneBulletLineDuration = new WaitForSeconds(DRONE_BULLET_LINE_DURATION); // ĳ��
        skillAttackDelayTimeSec = new WaitForSeconds(skillAttackDelay);             // ĳ��

        droneBulletLine = GetComponent<LineRenderer>();
        droneMuzzleFlash = GetComponent<Light2D>();
        droneBulletLine.enabled = false; // ���η����� ������Ʈ ��Ȱ��ȭ 
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

    private void DroneSkillActive() // ��� �̵� ���� Ȯ��
    {
        if (droneAttackCheck == null && droneRotateCheck == null)
        {
            isDroneStop = false;
            droneRotateCheck = StartCoroutine(DroneAroundRotate(playerObject.transform));
        }
    }

    private Define.CurrentSkillState DroneSkillAttack()
    {
        Collider2D[] enemyCollider = Physics2D.OverlapCircleAll(playerObject.transform.position, skillRange, enemyLayer); // �÷��̾� ���� 10������ ���� Ž��

        if (enemyCollider.Length > 0) // �� �迭�� 0���� ������
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
                lastTime += skillAttackDelay;   // ���� ��Ÿ�ӿ� Ÿ�Ϻ��� �ð����� �߰�
                yield return skillAttackDelayTimeSec;
            }
        }
        droneAttackCheck = null;
        OnCoolTime();
    }



    #region ��� ���� ȿ��

    /// <summary>
    /// ��� ȸ�� �ڷ�ƾ
    /// </summary>
    /// <param name="target">ȸ��Ȱ �������� �� ������Ʈ</param>
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
                //  transform.rotation = Quaternion.Euler(0, 0, deg); //����� �ٶ󺸰� ���� ����
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
    /// ��� ���� ����Ʈ
    /// </summary>
    /// <param name="enemyPosition">���� ��� ��ġ</param>
    /// <returns>droneBulletLineDuration ��ŭ ��ٸ� �� ���� ������ ����</returns>
    private IEnumerator DroneBulletEffect(Vector2 enemyPosition)
    {
        droneBulletLine.SetPosition(0, transform.position); // ���� ������ ���� ��ġ
        droneBulletLine.SetPosition(1, enemyPosition);      // ���� ������ ���� ��ġ

        droneMuzzleFlash.enabled = true; // 2D Light ������Ʈ Ȱ��ȭ
        droneBulletLine.enabled = true;  // ���� ������ ������Ʈ Ȱ��ȭ

        yield return droneBulletLineDuration;

        droneBulletLine.enabled = false; // ���� ������ ������Ʈ ��Ȱ��ȭ
        droneMuzzleFlash.enabled = false; // 2D Light ������Ʈ ��Ȱ��ȭ
    }

    #endregion
}
