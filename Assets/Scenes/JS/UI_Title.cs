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
        //�� �̵�
        //Debug.Log("���ӽ���");
    }
    void OnShowSettings(PointerEventData data)
    {
        //settingWindow.gameObject.SetActive(true);
        Debug.Log("�����˾�");
    }

    void OnQuitGame(PointerEventData data)
    {
        //���� ���� ����
        Debug.Log("��������");
    }
}
