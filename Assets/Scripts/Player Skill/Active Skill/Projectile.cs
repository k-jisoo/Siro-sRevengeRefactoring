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
    /// 투사체 초기 설정
    /// </summary>
    /// <param name="targetTag">목표 타겟</param>
    /// <param name="dir">투사체 방향</param>
    /// <param name="projectileDamage">투사체 공격력</param>
    /// <param name="projectileSpeed">투사체 속도</param>
    public void ProjectileInit(Define.StringTag targetTag, Vector2 dir, int projectileDamage = 0, float projectileSpeed = 0f)
    {
        this.targetTag = targetTag;
        this.projectileDamage = projectileDamage;
        this.projectileSpeed = projectileSpeed;
        this.dir = dir;
    }

    /// <summary>
    /// 투사체 소멸 공통함수
    /// </summary>
    protected void DisableObject()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 투사체 메모리 반납
    /// </summary>
    private void OnDisable()
    {
        CancelInvoke();         // 혹시 인보크 예외처리
        MemoryPoolManager.GetInstance().InputGameObject(gameObject);
    }
}
