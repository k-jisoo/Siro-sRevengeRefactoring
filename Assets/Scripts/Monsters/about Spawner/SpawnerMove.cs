using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerMove : MonoBehaviour
{
    private Transform target;

    private void Start()
    {
        target = this.gameObject.transform;
        Invoke(nameof(dealy), 3f);
       // target = Managers.StageManager.Player.GetComponent<Transform>();    
    }

    void dealy()
    {
        target = Managers.StageManager.Player.GetComponent<Transform>();
    }


    void Update()
    {
        gameObject.transform.position = target.position;
    }
}
