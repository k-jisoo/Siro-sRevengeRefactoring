using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int projectileDamage;
    private float projectileSpeed;
    private Vector2 dir;
    private Define.StringTag targetTag;

    protected Define.StringTag TargetTag { get { return targetTag; } }
    protected int ProjectileDamage { get { return projectileDamage; } }
    protected float ProjectileSpeed { get { return projectileSpeed; } }
    protected Vector2 Dir { get { return dir; } }

    /// <summary>
    /// ����ü �ʱ� ����
    /// </summary>
    /// <param name="targetTag">��ǥ Ÿ��</param>
    /// <param name="dir">����ü ����</param>
    /// <param name="projectileDamage">����ü ���ݷ�</param>
    /// <param name="projectileSpeed">����ü �ӵ�</param>
    public void ProjectileInit(Define.StringTag targetTag, Vector2 dir, int projectileDamage = 0, float projectileSpeed = 0f)
    {
        this.targetTag = targetTag;
        this.projectileDamage = projectileDamage;
        this.projectileSpeed = projectileSpeed;
        this.dir = dir;
    }

    /// <summary>
    /// ����ü �Ҹ� �����Լ�
    /// </summary>
    protected void DisableObject()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// ����ü �޸� �ݳ�
    /// </summary>
    private void OnDisable()
    {
        CancelInvoke();         // Ȥ�� �κ�ũ ����ó��
        MemoryPoolManager.GetInstance().InputGameObject(gameObject);
    }
}
