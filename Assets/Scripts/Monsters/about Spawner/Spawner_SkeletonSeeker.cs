using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_SkeletonSeeker : MonoBehaviour
{
    int[] dx = new int[] { 7, -10 };
    int[] dy = new int[] { 53, 47 };
    public GameObject Prefab;
    public GameObject[] Monster;

    bool state;
    int idx;
    public int MAX;

    // Start is called before the first frame update
    void Start()
    {
        //dealy();
        Invoke(nameof(dealy), 1f);
    }

    void dealy()
    {
        idx = 0;
        Monster = new GameObject[MAX];
        for (int i = 0; i < MAX; i++)
        {
            GameObject ob = Instantiate(Prefab);
            ob.GetComponent<Enemy>().EnemyInit(Managers.StageManager.Player);
            Monster[i] = ob;
            ob.SetActive(false);
        }
        state = true;
        StartCoroutine("Spawn");
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(0.1f);
        if (Monster[idx].activeInHierarchy == false)
        {
            Monster[idx].transform.position = new Vector3(dx[idx],dy[idx],0);
            Monster[idx].SetActive(true);
            state = false;
        }
        idx++;
        if (idx == MAX)
        {
            idx = 0;
            state = true;
        }
        StartCoroutine("Spawn");
    }

    public void go()
    {
        Monster[0].GetComponent<SkeletonSeekerController>().Ready();
        Monster[1].GetComponent<SkeletonSeekerController>().Ready();
    }
}
