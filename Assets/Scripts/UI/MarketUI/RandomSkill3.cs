using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomSkill3 : MonoBehaviour
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
        RandomInt = Random.Range(1, 5);


        if (RandomInt == 1)
        {
            skill.sprite = simage[0];
            stext.text = " <알파드론> 가격:100 구매하시겠습니까?";
            ItemName.text = "<알파드론>";
            product = 100;
            itemDetailExplan.text = "<알파드론> 성능이 뛰어난 공격형 드론이며 범위 안의 적들을 공격한다.";
            skillLevel = 0;


        }
        else if (RandomInt == 2)
        {

            skill.sprite = simage[1];
            stext.text = "  <겁쟁이> 가격:200 구매하시겠습니까?";
            ItemName.text = "<겁쟁이>";
            product = 200;
            itemDetailExplan.text = "<겁쟁이> 적으로부터 공격을 받으면 속도가 일시적으로 빨라진다.";
            skillLevel = 0;
        }
        else if (RandomInt == 3)
        {
            skill.sprite = simage[2];
            stext.text = "  <마검사> 가격:300 구매하시겠습니까?";
            ItemName.text = "<마검사>";
            product = 300;
            itemDetailExplan.text = "<마검사> 해당 스킬을 사용한 직후 공격력이 일시적으로 증가한다.";
            skillLevel = 0;
        }

        else if (RandomInt == 4)
        {
            skill.sprite = simage[3];
            stext.text = "  <응급치료> 가격:300 구매하시겠습니까?";
            ItemName.text = "<응급치료>";
            product = 300;
            itemDetailExplan.text = "<응급치료> 적으로부터 공격을 받으면 최대 체력에 비례하여 체력을 회복한다.";
            skillLevel = 0;
        }
    }

}