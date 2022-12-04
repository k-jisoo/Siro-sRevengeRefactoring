using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public Slider playerSlider;
    public Image playerRightSlider;
    public Image playerLeftSlider;
    public Slider bossSlider;
    public Text killCount;
    public Text goldAmount;
    public Image[] activeImages = new Image[5];
    public Image[] passiveImages = new Image[5];
    public int activeSkillIndex;
    public int passiveSkillIndex;
    public Color color;

    public List<Image> activeSkillImages = new List<Image>();
    public List<Image> passiveSkillImages = new List<Image>();

    public List<ActiveSkill> activeSkillLists = new List<ActiveSkill>();
    public List<PassiveSkill> passiveSkillLists = new List<PassiveSkill>();

    public void InitUI()
    {
        InitSkillImages();
        UpdateGoldText();
        UpdateKillCounts();
    }

    public void InitSkillImages()
    {
        activeSkillIndex = 0;
        passiveSkillIndex = 0;

        if (activeSkillImages.Count == 0)
        {
            for (int i = 0; i < activeImages.Length; i++)
            {
                ChangeAlpha(activeImages, i, 0f);
            }
        }

        if (passiveSkillImages.Count == 0)
        {
            for (int i = 0; i < passiveImages.Length; i++)
            {
                ChangeAlpha(passiveImages, i, 0f);
            }
        }
    }

    public void UpdatePlayerHpSlider(float currentHp, float maxHp)      //�÷��̾ ������ �޾��� �� ����.
    {
        if (currentHp / maxHp > 0.5)
        {
            playerLeftSlider.fillAmount = 1f;
            playerRightSlider.fillAmount = (currentHp + currentHp - maxHp) / maxHp;
        }
        else
        {
            playerRightSlider.fillAmount = 0;
            playerLeftSlider.fillAmount = (currentHp + currentHp) / maxHp;
        }
    }

    public void UpdateBossHpSlider(float currentHp, float maxHp)        //���� ���Ͱ� ������ �޾��� �� ����.
    {
        bossSlider.value = currentHp / maxHp;
    }
    public void UpdateGoldText()    //��� ȹ�� �� ����.
    {
        goldAmount.text = Managers.StageManager.Player.PlayerGold + "";
    }
    public void UpdateKillCounts()  //���� óġ �� ����.
    {
        killCount.text = ("" + Managers.StageManager.killCount);
    }
    public void InitKillText(string killCount)
    {
        this.killCount.text = killCount;
    }
    public void UpdateActiveSkills(Image image, ActiveSkill activeSkill)        //�������� ��ų ���Ž� ����.
    {
        activeImages[activeSkillIndex].sprite = image.sprite;                   //��ų �̹��� UI�� �߰�
        activeSkillLists.Add(activeSkill);                                      //��Ƽ�� ��ų �����ͼ� ����Ʈ�� ����
        ChangeAlpha(activeImages, activeSkillIndex, 1f);                            //�̹��� ���İ� 1�� �����ؼ� ���
        activeSkillIndex++;                                                     //index++
    }
    public void UpdatePassiveSkills(Image image, PassiveSkill passiveSkill)      //�������� ��ų ���Ž� ����.
    {
        passiveImages[passiveSkillIndex].sprite = image.sprite;
        passiveSkillLists.Add(passiveSkill);
        ChangeAlpha(passiveImages, passiveSkillIndex, 1f);
        passiveSkillIndex++;
    }

    public void CoolTimeProcess()
    {
        for (int i = 0; i < activeSkillLists.Count; i++)
        {
            if (activeSkillLists[i].currentSkillState == Define.CurrentSkillState.COOL_TIME)
            {
                ChangeAlpha(activeImages, i, 0.5f);
            }
            else
            {
                ChangeAlpha(activeImages, i, 1f);
            }
        }
    }

    public void InitBossSlider()
    {
        bossSlider.value = 1;
        Managers.StageManager.isBossAlive = true;
    }

    public void ChangeAlpha(Image[] image, int index, float alpha)
    {
        color = image[index].color;
        color.a = alpha;
        image[index].color = color;
    }
}
