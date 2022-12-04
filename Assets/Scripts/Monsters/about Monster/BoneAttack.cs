using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class BoneAttack :BasicMonsterController
{
    public GameObject Prefab;
    public GameObject[] Monster;
    int idx = 0;
    int MAX = 7;
    bool isAttack;

    private new void Start()
    {
        base.Start();

        Monster = new GameObject[MAX];
        for (int i = 0; i < MAX; i++)
        {
            int tmp = Random.Range(0, 2);
            GameObject ob = Instantiate(Prefab);
            if (tmp == 0)
                ob.GetComponent<SpriteRenderer>().color = new Color(150/255f, 210/255f, 255/255f, 255/255f);
            else ob.GetComponent<SpriteRenderer>().color = new Color(180/255f, 140/255f, 230/255f, 255/255f);
            ob.GetComponent<Enemy>().EnemyInit(Managers.StageManager.Player);
            Monster[i] = ob;
            ob.SetActive(false);
        }
        StartCoroutine("Spawn");
    }

    private void OnEnable()
    {
        state = State.Run;
        StartCoroutine("Spawn");

    }
    protected override void Attack()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(base.skillTime);
        string name = base.attackAudio.ToString().Substring(0, base.attackAudio.ToString().Length - 33);
        Debug.Log(name + "_Attack");
        Managers.Sound.PlaySFXAudio("Monster/" + name + "_Attack", base.attackAudio, base.volume, false);
        Monster[idx].transform.position = gameObject.transform.position;
        Monster[idx++].SetActive(true);
        if (idx == MAX)
            idx = 0;
        StartCoroutine("Spawn");
    }
}