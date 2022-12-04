using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerYJ : MonoBehaviour
{
    public TalkManager talkManager;
    public GameObject talkPanel;
    public Text easyTalk;
    public GameObject scanObject;
    public bool isAction;
    public int talkIndex;
    public GameObject marketUI;


    private void Start()
    {
        Managers.StageManager.shopManager = this;
        Managers.Sound.PlayBGMAudio("Bird1");
    }

    public void Action(GameObject scanOb)
    {
        scanObject = scanOb;
        ObjData A/*형식이나 변수처럼 이용*/ = scanObject.GetComponent<ObjData>();


        if(scanObject.CompareTag("Market"))
        {
            marketUI.gameObject.SetActive(true);
            Managers.StageManager.Player.PlayerController.bossDebuff = true;
        }
    
         

        else
        {
            print(A.id + " ," + A.isNPC);
            Talk(A.id, A.isNPC);
    
            talkPanel.SetActive(isAction); //true or fasle 
        }
        
    }



    void Talk(int id, bool isNPC)
    {

        
            string talkData = talkManager.GetTalk(id, talkIndex); //해당하는 문자열이 나온다. 

        if (talkData == null)
        {  isAction = false;
            FreezePlayer(false);
            talkIndex = 0;
            return; } // 이야기가 다 끝나고, 즉 인덱스가 다 돌아가면 대화창 내리기,
                                       // talkIndex와 대화의 문장 갯수를 비교해 끝 확인  
                                       //여기서 끝내면 뒤쪽 if문은 실행되지않음

        if (isNPC) //챕터별 상점 주인마다 id지정 
        {
            easyTalk.text = talkData;

        }

        else 
        {
            easyTalk.text = talkData;
        }
        isAction = true;
        FreezePlayer(true);
        talkIndex++; 
    }

    public void FreezePlayer(bool isFreeze)
    {
        Managers.StageManager.Player.PlayerController.bossDebuff = isFreeze;
    }
}
