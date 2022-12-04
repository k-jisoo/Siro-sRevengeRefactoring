using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;

public class BossSpawnEffect : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public TimelineAsset timeline;
    public Text bossEffectText;
    public Image bossEffectImage;
    public Sprite[] bossImageList;
    public CinemachineVirtualCamera bossCamera;

    #region 타임라인 함수
    public void Play()
    {
        // 현재 playableDirector에 등록되어 있는 타임라인을 실행
        playableDirector.Play();
    }
    public void PlayFromTimeline()
    {
        playableDirector.Play(timeline);
        Managers.Sound.PlayBGMAudio("BossSpawnBackGround");
    }
    #endregion

    #region 타임라인 시그널 함수
    public void TimeLineStartSignal()
    {
        Managers.StageManager.isSpawnOkay = false;
        Managers.StageManager.Player.PlayerController.isMoveable = false;
        Managers.StageManager.Player.PlayerController.bossDebuff = true;
        Managers.StageManager.Player.PlayerController.isAttackalble = false;
        UpdateBossEffectText();
        Managers.SkillEffectVolume.ChagnePostProcessProfile(null);
        Managers.StageManager.isSpawnOkay = true;
    }

    public void TimeLineEndSignal()
    {
        Managers.StageManager.isSpawnOkay = true;
        Managers.StageManager.Player.PlayerController.isMoveable = true;
        Managers.StageManager.Player.PlayerController.bossDebuff = false;
        Managers.StageManager.Player.PlayerController.isAttackalble = true;
        bossCamera.gameObject.SetActive(false);
    }
    #endregion

    public void UpdateBossEffectText()     
    {
        switch (Managers.StageManager.stage)
        {
            case Define.Stage.STAGE1:
                bossEffectText.text = "화가 많은 애벌레";
                bossEffectImage.sprite = bossImageList[0];
                break;
            case Define.Stage.STAGE2:
                bossEffectText.text = "까꿍기사";
                bossEffectImage.sprite = bossImageList[1];
                break;
            case Define.Stage.STAGE3:
                bossEffectText.text = "불타는지팡이";
                bossEffectImage.sprite = bossImageList[2];
                break;
            case Define.Stage.STAGE4:
                bossEffectText.text = "흑화한 농부";
                bossEffectImage.sprite = bossImageList[3];
                break;
            case Define.Stage.Boss:
                bossEffectText.text = "착했던 사령술사";
                bossEffectImage.sprite = bossImageList[4];
                break;

        }
    }

}
