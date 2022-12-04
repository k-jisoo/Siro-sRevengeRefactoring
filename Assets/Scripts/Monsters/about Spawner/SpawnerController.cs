using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{

    public GameObject spawnerBasic1;
    public GameObject spawnerBasic2;
    public GameObject spawnerElite;

    bool restart = false;

    private void Start()
    {
        spawnerBasic1.SetActive(true);
        spawnerBasic2.SetActive(true);
        spawnerElite.SetActive(true);

    }
    // Update is called once per frame
    void Update()
    {
        if (Managers.StageManager.isSpawnOkay == false&&restart==false)//&&Managers.StageManager.isBossAlive==true
        {
            Debug.Log("setFalse");
            spawnerBasic1.SetActive(false);
            spawnerBasic2.SetActive(false);
            spawnerElite.SetActive(false);
            restart = true;
        }

        if (Managers.StageManager.isSpawnOkay==true&&Managers.StageManager.isBossAlive == true && restart == true)
        {
            Debug.Log("»£√‚");
            spawnerBasic1.SetActive(true);
            spawnerBasic2.SetActive(true);
            spawnerElite.SetActive(true);
            restart = false;
        }
    }
}
