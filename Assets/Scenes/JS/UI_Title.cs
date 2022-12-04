using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Title : UI_Base
{
    public GameObject buttonStart;
    public GameObject buttonSetting;
    public GameObject buttonClose;
    public Canvas settingWindow;

    // Start is called before the first frame update
    void Start()
    {
        BindEvent(buttonStart, OnGameStart, UIEvent.Click);
        BindEvent(buttonSetting, OnShowSettings, UIEvent.Click);
        BindEvent(buttonClose, OnQuitGame, UIEvent.Click);
    }

    void OnGameStart(PointerEventData data)
    {
        //씬 이동
        //Debug.Log("게임시작");
    }
    void OnShowSettings(PointerEventData data)
    {
        //settingWindow.gameObject.SetActive(true);
        Debug.Log("세팅팝업");
    }

    void OnQuitGame(PointerEventData data)
    {
        //게임 종료 구현
        Debug.Log("게임종료");
    }
}
