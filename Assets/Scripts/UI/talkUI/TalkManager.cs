using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    [SerializeField] ObjData id;


    Dictionary<int, string[]> talkData;
    //Ư�� ������Ʈ�� �ش��ϴ� id�� �ҷ��� ��ȭ�� ��ġ��Ű�� Ű�� ������ ���� ( Ÿ�� �ΰ� �ʿ�, string �� �迭�� �ִ´�.)



    private void Start()
    {
        talkData = new Dictionary<int, string[]>(); // ���� ���� 
        GenerateData(); //�޼ҵ� �ҷ�����
    }

    void GenerateData()
    {
        switch (Managers.StageManager.stage)
        {
            case Define.Stage.STORE1:
                talkData.Add(1, new string[] { "...��? ������ �����ڴ� �������� ���±�.", "�� ������ ���� �� ������ ���ٴ�, �������� �ƴϱ���.", "�ڳ� ���� �ٺ��� ���� ������ ���� ���⼭ ������ �ϴ� �Դ�.", "�ʿ��� �� ���� �ٵ�, Ư���� �ΰ� ������.", "���� �������� ���� ������.", "�̰� ���� �غ� �� �Ǹ� ���� ���� ��Ż�� �ٽ� ����. " });
                id.id = 1;
                break;

            case Define.Stage.STORE2:
                talkData.Add(2, new string[] { "���, �� ���ĳ����°�? �ʽ��� ġ�� ����ϱ�.", "���� �༮���� ���� ����ġ �����Ծ�. �����Ͻð�." });
                id.id = 2;
                break;

            case Define.Stage.STORE3:
                talkData.Add(3, new string[] { " ��ġ�� �ʾҳ�? ����ִ� �ڳ� ���� ��� ���� �� �̸��� ����� ���̾�.", "������ �決. �̷��Ա��� �뾲�� �����̾� �𸣰����� ���� �� �縮�Գ�." });
                id.id = 3;
                break;

            case Define.Stage.STORE4:
                talkData.Add(4, new string[] { "...�� �ʸӿ� �ִ� ���� �������� �������� ���߳�.", "�ҽ��� ���� ���� ����� �޲����.","���� ������ ���� �ε��� ������ ���⼭ �����̳� �ϸ鼭 ��ġ�� �ִٸ�,", " ������ ���� �� ��ٳ�. �ε� �����ϽðԳ�." });
                id.id = 4;
                break;
              
        }

    }



    public string GetTalk(int id, int talkIndex) //��ȯ���� ���ڿ�  
                                                 //������ ��ȭ ������ ��ȯ�ϴ� �Լ�. 
    {  

        if (talkIndex == talkData[id].Length) //talkindex�� ��ȭ�� ���� ���� ���Ͽ� �� Ȯ��


            return null;
   
            else
               return talkData[id][talkIndex]; }//Ű ��ȯ�� �� ���
                                                // GetTalk�Լ��� �� ���徿 �ҷ��� ���� (id �� ��ȭ�� �ҷ����� , talklIndex�� ��ȭ�� �� ������ �ҷ��´�) 




  

    //GenaarteDate�� talkDate�� �� ���徿 return�ؼ� ��ȯ���ش�. 
}



