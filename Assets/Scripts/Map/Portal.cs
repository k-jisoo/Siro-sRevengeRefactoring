using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    
    bool inPortal =false;
    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Portal")
        {
            if (Input.GetKey(KeyCode.Space)&& !inPortal)
            {
                inPortal = true;
                StartCoroutine(ToStore());
                Managers.UI.InitKillText("X");
                Managers.StageManager.bossCount--;
            }

        }
        else if (collision.gameObject.tag == "StorePortal")
        {
            if (Input.GetKey(KeyCode.Space)&&!inPortal)
            {
                inPortal = true;
                StartCoroutine(SceneChange());
                Managers.StageManager.InitMonsterCounter();
                Managers.UI.InitKillText(Managers.StageManager.killCount.ToString());
                Managers.UI.InitBossSlider();
            }
        }

    }
    IEnumerator ToStore()
    {
        switch (Managers.StageManager.stage)
        {
            case Define.Stage.STAGE1:
                Managers.StageManager.stage = Define.Stage.STORE1;
                break;
            case Define.Stage.STAGE2:
                Managers.StageManager.stage = Define.Stage.STORE2;
                break;
            case Define.Stage.STAGE3:
                Managers.StageManager.stage = Define.Stage.STORE3;
                break;
            case Define.Stage.STAGE4:
                Managers.StageManager.stage = Define.Stage.STORE4;
                break;
        }
        Sound();
        yield return new WaitForSeconds(1f);
        inPortal = false;
        transform.position = new Vector2(0, 0);
        MemoryPoolManager.GetInstance().InitPool();
        Managers.SkillEffectVolume.ChagnePostProcessProfile(null);

        //스포너 관련 함수 초기화
        Managers.StageManager.isSpawnOkay = true;
        Managers.StageManager.isBossAlive = true;

        LoadingScene.LoadScene("JinminStore");
    }

    IEnumerator SceneChange()
    {
        Sound();
        yield return new WaitForSeconds(1f);
        transform.position = new Vector2(0, 0);
        inPortal = false;
        MemoryPoolManager.GetInstance().InitPool();
        Managers.SkillEffectVolume.ChagnePostProcessProfile(null);
        switch (Managers.StageManager.stage)
        {
            case Define.Stage.STORE1:
                LoadingScene.LoadScene("Stage2");
                Managers.StageManager.stage = Define.Stage.STAGE2;
                break;
            case Define.Stage.STORE2:
                LoadingScene.LoadScene("Stage3");
                Managers.StageManager.stage = Define.Stage.STAGE3;
                break;
            case Define.Stage.STORE3:
                LoadingScene.LoadScene("Stage4");
                Managers.StageManager.stage = Define.Stage.STAGE4;
                break;
            case Define.Stage.STORE4:
                LoadingScene.LoadScene("Stage5");
                Managers.StageManager.stage = Define.Stage.Boss;
                break;
        }
    }
    private void Sound()
    {
        Managers.Sound.PlaySFXAudio("SubBoss/portal_SFX");
    }

}


