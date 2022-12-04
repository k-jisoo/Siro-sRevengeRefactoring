using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanIBuy : MonoBehaviour

{
    [SerializeField] RandomSkill RandomSkillSlot1;
    [SerializeField] RandomSkill2 RandomSkillSlot2;
    [SerializeField] RandomSkill3 RandomSkillSlot3;



    readonly string DEFAULT_SKILL_PATH = "Player_Skill/";
    private bool isActiveSkill;
    GameObject skillObject;
    public Text money;
    public Button btn;
    public GameObject Yes;
    public GameObject No;
    public GameObject Up;
    public int item;
    public float Speed;
    public Button SelectBtn1;
    public Button SelectBtn2;
    public Button SelectBtn3;
    int skillLevel;
    Image selectImage;
    AudioSource audioSoure;
    public Image soldout;

    public void Start()
    {
        audioSoure = GetComponent<AudioSource>();
        Yes.SetActive(false);
        No.SetActive(false);
        Up.SetActive(false);
        btn.onClick.AddListener(Answer);
        


        SelectBtn1.onClick.AddListener(Buy1);
        SelectBtn2.onClick.AddListener(Buy2);
        SelectBtn3.onClick.AddListener(Buy3);

    }



    void HideAnswerY() { Yes.SetActive(false); }
    void HideAnswerN() { No.SetActive(false); }

    void HideAnswerU() { Up.SetActive(false); }

    public void Answer()
    {
        audioSoure.Play();

        if (Managers.StageManager.Player.PlayerGold >= item)
        { 
            Managers.StageManager.Player.PlayerGold -= item;



            if (!Managers.StageManager.Player.skillList.TryGetValue(skillObject.name, out PlayerSkill updateSkill))
            {
                GameObject newSkillObject = Instantiate(skillObject, Managers.StageManager.Player.transform);

                Yes.SetActive(true);
                Invoke("HideAnswerY", Speed);

                  skillLevel++;
                if (isActiveSkill)
                {
        
                    ActiveSkill activeSkill = newSkillObject.GetComponent<ActiveSkill>();
                    activeSkill.Init(Managers.StageManager.Player);
                    Managers.StageManager.Player.playerActiveSkills.Add(Managers.StageManager.Player.ActiveSkillSlot_Index++, activeSkill);
                    Managers.UI.UpdateActiveSkills(selectImage, activeSkill);
                    print("스킬구매" + selectImage.ToString() + activeSkill.ToString());
                }
                else
                {
                    PassiveSkill passiveSkill = newSkillObject.GetComponent<PassiveSkill>();
                    passiveSkill.Init(Managers.StageManager.Player);
                    passiveSkill.OnActive();
                    Managers.UI.UpdatePassiveSkills(selectImage, passiveSkill);
                }

              
                Managers.StageManager.Player.skillList.Add(skillObject.name, newSkillObject.GetComponent<PlayerSkill>());
            }

            else // 강화
            {
                Up.SetActive(true);
                Invoke("HideAnswerU", Speed);


                if (updateSkill.GetComponent<ActiveSkill>() != null)
                {
                    updateSkill.GetComponent<ActiveSkill>().Upgrade();
                }
                else
                {

                    updateSkill.GetComponent<PassiveSkill>().Upgrade();
                }
                updateSkill.SkillLevel++;
            }
            money.text = Managers.StageManager.Player.PlayerGold.ToString();
            selectImage = soldout;
        }


        else
        {
            No.SetActive(true);
            Invoke("HideAnswerN", Speed);
        }

    }

    void Buy1()
    {
        item = RandomSkillSlot1.product;
        selectImage = RandomSkillSlot1.skill;
        skillLevel = RandomSkillSlot1.skillLevel;

        switch (RandomSkillSlot1.RandomInt)
        {
            case 1: skillObject = MakeSkillObject("ThunderSlash Skill", true);break;
            case 2: skillObject = MakeSkillObject("FlameStrike Skill", true); break;
            case 3: skillObject = MakeSkillObject("WindDash Skill", true); break;
        }

        
    }

    void Buy2()
    {
        item = RandomSkillSlot2.product;
        selectImage = RandomSkillSlot2.skill;
        skillLevel = RandomSkillSlot2.skillLevel;

        switch (RandomSkillSlot2.RandomInt)
        {
            case 1: skillObject = MakeSkillObject("ThrowingKnife Skill", true);break;
            case 2: skillObject = MakeSkillObject("Barrier Skill", true); break;
            case 3: skillObject = MakeSkillObject("HourGlass Skill", false); break;
              
           
        }
    }

    void Buy3()
    {
        item = RandomSkillSlot3.product;
       selectImage = RandomSkillSlot3.skill;
       skillLevel = RandomSkillSlot3.skillLevel;

        switch (RandomSkillSlot3.RandomInt)
        {
            case 1: skillObject = MakeSkillObject("Drone Skill", false); break;
            case 2: skillObject = MakeSkillObject("Coward Skill", false); break;
            case 3: skillObject = MakeSkillObject("SpellBlade Skill", false); break;
            case 4: skillObject = MakeSkillObject("FirstAid Skill", false); break;
        }
    }

    private GameObject MakeSkillObject(string skillObjectName, bool isActiveSkill)
    {
        skillObject = Managers.Resource.GetPerfabGameObject(DEFAULT_SKILL_PATH + skillObjectName);
        this.isActiveSkill = isActiveSkill;

        return skillObject;
    }



}



