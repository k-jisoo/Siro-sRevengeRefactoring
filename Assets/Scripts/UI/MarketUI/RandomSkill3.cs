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
    public Text stext; //����â�� �ߴ� �ؽ�Ʈ
    public Text itemDetailExplan; //���� ���� 
    public int product; //���� ����
    public int skillLevel;


    // Start is called before the first frame update
    void Start()
    {
        RandomInt = Random.Range(1, 5);


        if (RandomInt == 1)
        {
            skill.sprite = simage[0];
            stext.text = " <���ĵ��> ����:100 �����Ͻðڽ��ϱ�?";
            ItemName.text = "<���ĵ��>";
            product = 100;
            itemDetailExplan.text = "<���ĵ��> ������ �پ ������ ����̸� ���� ���� ������ �����Ѵ�.";
            skillLevel = 0;


        }
        else if (RandomInt == 2)
        {

            skill.sprite = simage[1];
            stext.text = "  <������> ����:200 �����Ͻðڽ��ϱ�?";
            ItemName.text = "<������>";
            product = 200;
            itemDetailExplan.text = "<������> �����κ��� ������ ������ �ӵ��� �Ͻ������� ��������.";
            skillLevel = 0;
        }
        else if (RandomInt == 3)
        {
            skill.sprite = simage[2];
            stext.text = "  <���˻�> ����:300 �����Ͻðڽ��ϱ�?";
            ItemName.text = "<���˻�>";
            product = 300;
            itemDetailExplan.text = "<���˻�> �ش� ��ų�� ����� ���� ���ݷ��� �Ͻ������� �����Ѵ�.";
            skillLevel = 0;
        }

        else if (RandomInt == 4)
        {
            skill.sprite = simage[3];
            stext.text = "  <����ġ��> ����:300 �����Ͻðڽ��ϱ�?";
            ItemName.text = "<����ġ��>";
            product = 300;
            itemDetailExplan.text = "<����ġ��> �����κ��� ������ ������ �ִ� ü�¿� ����Ͽ� ü���� ȸ���Ѵ�.";
            skillLevel = 0;
        }
    }

}