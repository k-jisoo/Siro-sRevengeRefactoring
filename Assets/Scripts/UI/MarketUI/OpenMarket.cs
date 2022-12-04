using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenMarket : MonoBehaviour
{

   public  Button mbtn;
   public GameObject MarketTool;
    // Start is called before the first frame update
    void Awake()
    {
        
        mbtn.onClick.AddListener(activeMarket);
    }

    // Update is called once per frame
    public void activeMarket()
    {
        MarketTool.SetActive(true);
    }
}
