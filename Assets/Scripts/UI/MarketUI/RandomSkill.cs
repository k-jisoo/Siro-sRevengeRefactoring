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
    public Text stext; //����â�� �ߴ� �ؽ�Ʈ
    public Text itemDetailExplan; //���� ���� 
    public int product; //���� ����
    public int skillLevel; 



    // Start is called before the first frame update
    void Start()

        
    {
        RandomInt = Random.Range(1,4);

   
        if (RandomInt == 1)
        {
            skill.sprite = simage[0];
            stext.text = " <�����ϼ�> ����:100 �����Ͻðڽ��ϱ�?";
            ItemName.text = "�����ϼ�";
            product = 100;
            itemDetailExplan.text = "<�����ϼ�> �÷��̾� ���ؿ��� Ư�� �ݰ� ������ �����Ͽ� ������ �Ѵ�.";
            skillLevel = 0; 
            

        }
        else if (RandomInt == 2)
        {

            skill.sprite = simage[1];
            stext.text = "  <ȭ�����> ����:200 �����Ͻðڽ��ϱ�?";
            ItemName.text = "ȭ�����";
            product = 200;
            itemDetailExplan.text = "<ȭ�����> �÷��̾� ���ؿ��� �����·� �ұ���� ��ȯ�Ͽ� ������ �Ѵ�.";
            skillLevel = 0;
        }
        else if (RandomInt == 3)
        {
            skill.sprite = simage[2];
            stext.text = "  <��ǳ��> ����:300 �����Ͻðڽ��ϱ�?";
            ItemName.text = "<��ǳ��>";
            product = 300;
            itemDetailExplan.text = "<��ǳ��> �÷��̾ ���������� ���� �ִ� �������� �����Ͽ� �����Ѵ�.";
            skillLevel = 0;
        }



    }



}

   

   