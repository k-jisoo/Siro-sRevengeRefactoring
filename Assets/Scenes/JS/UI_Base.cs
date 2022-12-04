using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public abstract class UI_Base : MonoBehaviour
{
    public enum UIEvent
    {
        Click,
        Enter,
        Exit,
        Up
    }

    public static void BindEvent(GameObject uiObject, Action<PointerEventData> action, UIEvent type = UIEvent.Click)
    {
        UIPointerHandler evt = GetAddedComponent<UIPointerHandler>(uiObject);

        switch (type)
        {
            case UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
        }
    }

    public static T GetAddedComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }
}
