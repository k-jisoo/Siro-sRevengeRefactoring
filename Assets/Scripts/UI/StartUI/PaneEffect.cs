using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PaneEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlaneEffectInit();
    }
    
    private void PlaneEffectInit()
    {
        transform.localScale = Vector2.zero;
    }

    public void Open()
    {
        transform.LeanScale(Vector2.one, 0.8f);
    }
    // Update is called once per frame
   public void Close()
    {
        transform.LeanScale(Vector2.zero,0.8f).setEaseInBack();
    }
}
