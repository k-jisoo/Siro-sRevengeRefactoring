using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class StageManager : MonoBehaviour
{
    public Player Player;
    GameObject portal;
    public int killCount;      //���� ų ī��Ʈ
    public Define.Stage stage;        //���� ��������
    public GameObject[] coins = new GameObject[3];
    public int bossCount = 0;
    public bool isBossSpawn = false;


    #region ���� ����
    public GameManagerYJ shopManager;
    #endregion

    #region ���� ������ ����
    public int monsterCounter = 0;
    public bool isSpawnOkay = true;
    public bool isBossAlive = true;
    #endregion

    #region �� Fade ���� ����
    public TextMeshProUGUI mainTitleText;
    public TextMeshProUGUI subTitleText;
    public Animator sceneAnimator;
    public GameObject sceneMovie;
    #endregion

    #region ����Ƽ �Լ�

    private void Start()
    {
        SetStageKillCount();
        stage = Define.Stage.STAGE1;
        SenecFadeEffect();
        monsterCounter = 0;
        isSpawnOkay = true;
        isBossAlive = true;
}

    #endregion
    public void InitMonsterCounter()
    {
        monsterCounter = 0;
    }

    public void DecreaseKillCount()     //ųī��Ʈ�� ���̴� ������� �����Ϸ��� ��. �÷��̾ ���͸� ���̸� ����.
    {
        if (killCount == 0)
        {
            isSpawnOkay = false;
            return;
        }
        killCount--;
        Managers.UI.UpdateKillCounts();
    }
    public bool IsStageCleared()        //���������� Ŭ���� �Ǿ��°� Ȯ���ϴ� �Լ�.
    {
        if (killCount <= 0)
            return true;
        else
            return false;
    }
    public void SetStageKillCount()     //ų ī��Ʈ�� 100���� �ʱ�ȭ �ϴ� �Լ�.
    {
        killCount = 100;
    }
    public void ChangeStage()                   //�� �̵��� �����ؾ� �ϴ� �Լ�. ���������� 1�� ������Ų��.
    {
        stage++;
    }
    public int ReturnKillCount()
    {
        return killCount;
    }

    public void SenecFadeEffect()
    {
        switch (stage)
        {
            case Define.Stage.STAGE1:
                mainTitleText.text = "�� 1��";
                subTitleText.text = "���� ����";
                break;
            case Define.Stage.STAGE2:
                mainTitleText.text = "�� 2��";
                subTitleText.text = "���ָ� ����";
                break;
            case Define.Stage.STAGE3:
                mainTitleText.text = "�� 3��";
                subTitleText.text = "���ְ� ���ְ�";
                break;
            case Define.Stage.STAGE4:
                mainTitleText.text = "�� 4��";
                subTitleText.text = "������ �ʴ� ����";
                break;
            case Define.Stage.Boss:
                mainTitleText.text = "�� 5��";
                subTitleText.text = "�÷��� ����";
                break;

            case Define.Stage.STORE1:
                mainTitleText.text = "�޽�ó";
                subTitleText.text = "���� ���";
                break;
            case Define.Stage.STORE2:
                mainTitleText.text = "�޽�ó";
                subTitleText.text = "Ǫ���� ��";
                break;
            case Define.Stage.STORE3:
                mainTitleText.text = "�޽�ó";
                subTitleText.text = "������ ��������";
                break;
            case Define.Stage.STORE4:
                mainTitleText.text = "�޽�ó";
                subTitleText.text = "������ �޳���";
                break;
        }
        StartCoroutine(BackGroundSound(stage));
        sceneAnimator.SetTrigger("Movie Start");
        Invoke(nameof(FadeEffect), 0.5f);
    }

    public void IsBossAlive(float currentHp)
    {
        if (currentHp <= 0f)
        {
            isSpawnOkay = false;
            isBossAlive = false;
        }
        else
        {
            isBossAlive = true;
        }
    }

    private void FadeEffect()
    {
        Managers.Sound.PlaySFXAudio("Etc/SceneChangeSFX");
    }
    IEnumerator BackGroundSound(Define.Stage stage)
    {
        yield return new WaitForSeconds(1.5f);
        switch (stage)
        {
            case Define.Stage.STAGE1:
                Managers.Sound.PlayBGMAudio("Stage1BackGround", null, 0.5f, true);
                break;
            case Define.Stage.STAGE2:
                Managers.Sound.PlayBGMAudio("Stage2BackGround", null, 0.5f, true);
                break;
            case Define.Stage.STAGE3:
                Managers.Sound.PlayBGMAudio("Stage3BackGround", null, 0.5f, true);
                break;
            case Define.Stage.STAGE4:
                Managers.Sound.PlayBGMAudio("Stage4BackGround", null, 0.5f, true);
                break;
            case Define.Stage.Boss:
                Managers.Sound.PlayBGMAudio("Stage5BackGround", null, 0.5f, true);
                break;
        }
    }
}
