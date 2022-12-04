using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZone : MonoBehaviour
{
    BossSpawnEffect bossSpawnEffect;
    Boss bossObejct;

    private void Start()
    {
        bossSpawnEffect = FindObjectOfType<BossSpawnEffect>();
        bossObejct = FindObjectOfType<Boss>();
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.CompareTag(Define.StringTag.Player.ToString()))
        {
            Managers.StageManager.isBossSpawn = true;
            Managers.UI.bossSlider.gameObject.SetActive(true);
            Managers.UI.InitBossSlider();
            Managers.CameraManager.SetFollow(bossObejct.transform);
            bossSpawnEffect.PlayFromTimeline();
            bossObejct.BossSetBattle();
            Destroy(gameObject);
        }
    }
}
