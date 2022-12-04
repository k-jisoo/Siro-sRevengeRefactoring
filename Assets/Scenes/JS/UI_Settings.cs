using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Settings : UI_Base
{
    public Slider soundSlider;
    public Canvas settingWindow;
    public GameObject buttonCloseWindow;
    public GameObject buttonStoreSettings;

    // Start is called before the first frame update
    void Start()
    {
        BindEvent(buttonStoreSettings, OnStoreSettings, UIEvent.Click);
        BindEvent(buttonCloseWindow, OnClosePopup, UIEvent.Click);
    }

    void OnClosePopup(PointerEventData data)
    {
        //Managers.UI.ClosePopUpUI(settingWindow);
    }
    void OnStoreSettings(PointerEventData data)
    {
        //Managers.UI.ClosePopUpUI(settingWindow);
    }
}
