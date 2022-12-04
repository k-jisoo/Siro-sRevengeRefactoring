using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnSizeUp : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler //������ �ø� �� ������Ʈ ��ȯ ȿ��
{

 
    Vector3 defaultScale;
    public Transform buttonScale;
    // Start is called before the first frame update
    void Start()
    {
        
        defaultScale = buttonScale.localScale;
    }

    // Update is called once per frame
    public void OnPointerEnter(PointerEventData eventData)
    {
       
        buttonScale.localScale = defaultScale * 1.1f;
       
    }

    public void OnPointerExit(PointerEventData envetData)
    {
        
        buttonScale.localScale = defaultScale; //�����Ͱ� �ش� ������Ʈ���� ��� �� btn�ǽ������� ������� �ǵ�����.
      
    }
}
