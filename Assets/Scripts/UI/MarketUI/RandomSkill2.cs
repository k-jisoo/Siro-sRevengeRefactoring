using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomSkill2 : MonoBehaviour
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
        RandomInt = Random.Range(1, 4);


        if (RandomInt == 1)
        {
            skill.sprite = simage[0];
            stext.text = " <표창검> 가격:100 구매하시겠습니까?";
            ItemName.text = "<표창검>";
            product = 100;
            itemDetailExplan.text = "<표창검> 플레이어가 마지막으로 보고 있는 방향으로 표창검을 발사한다.";
            skillLevel = 0;

        }
        else if (RandomInt == 2)
        {

            skill.sprite = simage[1];
            stext.text = "  <XT-쉴드> 가격:200 구매하시겠습니까?";
            ItemName.text = "<XT-쉴드>";
            product = 200;
            itemDetailExplan.text = "<XT-쉴드> 플레이어를 감싸는 방벽이 생성되어 적과 접촉시 공격한다.";
            skillLevel = 0;
        }
        else if (RandomInt == 3)
        {
            skill.sprite = simage[2];
            stext.text = "  <모래시계> 가격:300 구매하시겠습니까?";
            ItemName.text = "<모래시계>";
            product = 300;
            itemDetailExplan.text = "<모래시계> 현재 체력이 30%이하로 떨어지면 시간이 정지된다.";
            skillLevel = 0;
        }


    }
}
   