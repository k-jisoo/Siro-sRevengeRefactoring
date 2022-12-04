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

    public void UpdatePlayerHpSlider(float currentHp, float maxHp)      //플레이어가 데미지 받았을 때 실행.
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

    public void UpdateBossHpSlider(float currentHp, float maxHp)        //보스 몬스터가 데미지 받았을 때 실행.
    {
        bossSlider.value = currentHp / maxHp;
    }
    public void UpdateGoldText()    //골드 획득 시 실행.
    {
        goldAmount.text = Managers.StageManager.Player.PlayerGold + "";
    }
    public void UpdateKillCounts()  //몬스터 처치 시 실행.
    {
        killCount.text = ("" + Managers.StageManager.killCount);
    }
    public void InitKillText(string killCount)
    {
        this.killCount.text = killCount;
    }
    public void UpdateActiveSkills(Image image, ActiveSkill activeSkill)        //상점에서 스킬 구매시 실행.
    {
        activeImages[activeSkillIndex].sprite = image.sprite;                   //스킬 이미지 UI에 추가
        activeSkillLists.Add(activeSkill);                                      //액티브 스킬 가져와서 리스트에 저장
        ChangeAlpha(activeImages, activeSkillIndex, 1f);                            //이미지 알파값 1로 변경해서 띄움
        activeSkillIndex++;                                                     //index++
    }
    public void UpdatePassiveSkills(Image image, PassiveSkill passiveSkill)      //상점에서 스킬 구매시 실행.
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
