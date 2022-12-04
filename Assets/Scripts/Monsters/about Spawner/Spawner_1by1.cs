using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_1by1 : MonoBehaviour
{
    public GameObject Prefab;
    public float spawnRateMin;
    public float spawnRateMax;

    private float spawnRate;

    int MaxCnt = 100;

    bool spawnerRestart = false;

    void Start()
    {
        StartCoroutine("Spawn");
        spawnerRestart = true;
        //Invoke(nameof(dealy), 3f);
    }

    /*void dealy()
    {
        StartCoroutine("Spawn");
        spawnerRestart = true;
    }*/

    private void OnEnable()
    {
        Debug.Log("spawnerRestart : " + spawnerRestart);
        if (spawnerRestart == true)
        {
            spawnRateMin = 12.0f;
            spawnRateMax = 15.0f;

            Debug.Log("spawn restart!");
            Debug.Log("1by1 SpawnRate Min/Max : " + spawnRateMin + " / " + spawnRateMax);
            StartCoroutine("Spawn");
        }
    }

    IEnumerator Spawn()
    {
        if (Managers.StageManager.monsterCounter++ <= MaxCnt)
        {
            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
            yield return new WaitForSeconds(spawnRate);
            GameObject ob = MemoryPoolManager.GetInstance().OutputGameObject(Prefab, "Monsters/Stage Monster/" + Prefab.name, this.transform.position, Quaternion.identity);
            ob.GetComponent<Enemy>().EnemyInit(Managers.StageManager.Player);
            ob.SetActive(true);
            StartCoroutine("Spawn");
        }
    }
}
