using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnSizeUp : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler //포인터 올릴 시 오브젝트 변환 효과
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
        
        buttonScale.localScale = defaultScale; //포인터가 해당 오브젝트에서 벗어날 시 btn의스케일을 원래대로 되돌린다.
      
    }
}
