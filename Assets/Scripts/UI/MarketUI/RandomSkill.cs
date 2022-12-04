using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomSkill : MonoBehaviour
{


    public Image skill;
    public Sprite[] simage;
    public Text ItemName;
    public int RandomInt;
    public Text stext; //구매창에 뜨는 텍스트
    public Text itemDetailExplan; //무기 설명 
    public int product; //물건 가격
    public int skillLevel; 



    // Start is called before the first frame update
    void Start()

        
    {
        RandomInt = Random.Range(1,4);

   
        if (RandomInt == 1)
        {
            skill.sprite = simage[0];
            stext.text = " <벽력일섬> 가격:100 구매하시겠습니까?";
            ItemName.text = "벽력일섬";
            product = 100;
            itemDetailExplan.text = "<벽력일섬> 플레이어 기준에서 특정 반경 적에게 돌진하여 공격을 한다.";
            skillLevel = 0; 
            

        }
        else if (RandomInt == 2)
        {

            skill.sprite = simage[1];
            stext.text = "  <화염기둥> 가격:200 구매하시겠습니까?";
            ItemName.text = "화염기둥";
            product = 200;
            itemDetailExplan.text = "<화염기둥> 플레이어 기준에서 원형태로 불기둥을 소환하여 공격을 한다.";
            skillLevel = 0;
        }
        else if (RandomInt == 3)
        {
            skill.sprite = simage[2];
            stext.text = "  <질풍참> 가격:300 구매하시겠습니까?";
            ItemName.text = "<질풍참>";
            product = 300;
            itemDetailExplan.text = "<질풍참> 플레이어가 마지막으로 보고 있는 방향으로 돌진하여 공격한다.";
            skillLevel = 0;
        }



    }



}

   

   