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
    public Text stext; //����â�� �ߴ� �ؽ�Ʈ
    public Text itemDetailExplan; //���� ���� 
    public int product; //���� ����
    public int skillLevel;


    // Start is called before the first frame update
    void Start()
    {
        RandomInt = Random.Range(1, 4);


        if (RandomInt == 1)
        {
            skill.sprite = simage[0];
            stext.text = " <ǥâ��> ����:100 �����Ͻðڽ��ϱ�?";
            ItemName.text = "<ǥâ��>";
            product = 100;
            itemDetailExplan.text = "<ǥâ��> �÷��̾ ���������� ���� �ִ� �������� ǥâ���� �߻��Ѵ�.";
            skillLevel = 0;

        }
        else if (RandomInt == 2)
        {

            skill.sprite = simage[1];
            stext.text = "  <XT-����> ����:200 �����Ͻðڽ��ϱ�?";
            ItemName.text = "<XT-����>";
            product = 200;
            itemDetailExplan.text = "<XT-����> �÷��̾ ���δ� �溮�� �����Ǿ� ���� ���˽� �����Ѵ�.";
            skillLevel = 0;
        }
        else if (RandomInt == 3)
        {
            skill.sprite = simage[2];
            stext.text = "  <�𷡽ð�> ����:300 �����Ͻðڽ��ϱ�?";
            ItemName.text = "<�𷡽ð�>";
            product = 300;
            itemDetailExplan.text = "<�𷡽ð�> ���� ü���� 30%���Ϸ� �������� �ð��� �����ȴ�.";
            skillLevel = 0;
        }


    }
}
   