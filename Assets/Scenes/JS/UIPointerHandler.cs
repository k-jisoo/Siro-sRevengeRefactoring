using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPointerHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    // �� �̺�Ʈ�� ���� ��������Ʈ
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnEnterHandler = null;
    public Action<PointerEventData> OnExitHandler = null;
    public Action<PointerEventData> OnUpHandler = null;

    // Ŭ�� �̺�Ʈ
    public void OnPointerClick(PointerEventData eventData)      //null�� �ƴϸ� ����.
        => OnClickHandler?.Invoke(eventData);

    // ���콺 ���� �̺�Ʈ (���콺�� ��ư ���� �ö� �� ����)
    public void OnPointerEnter(PointerEventData eventData)
        => OnEnterHandler?.Invoke(eventData);

    // ���콺 ���� ���� (���콺�� ��ư�� ��� �� ����)
    public void OnPointerExit(PointerEventData eventData)
        => OnExitHandler?.Invoke(eventData);

    // ���콺 Ŭ��(�Ǵ� �巡��)�� ���� ������ ����
    public void OnPointerUp(PointerEventData eventData)
        => OnUpHandler?.Invoke(eventData);
}
