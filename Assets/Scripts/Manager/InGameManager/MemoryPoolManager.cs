using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPoolManager : MonoBehaviour
{
    private static MemoryPoolManager instance;

    private Dictionary<string, Queue<GameObject>> pools;

    private void Awake()
    {
        SetInstance();
    }
    private void SetInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    public static MemoryPoolManager GetInstance()
    {
        return instance;
    }


    /// <summary>
    /// 오브젝트 반납(넣는다)
    /// </summary>
    /// <param name="gameObject">반납 SetActive(ture)오브젝트</param>
    public void InputGameObject(GameObject gameObject)
    {
        // 반납할 Queue가 없으면 그대로 실행 X
        if (!pools.ContainsKey(gameObject.name)) return;

      //  print(gameObject.name + " 반납");
        gameObject.SetActive(false);
        pools[gameObject.name].Enqueue(gameObject);
    }

    /// <summary>
    /// 오브젝트 사용(꺼낸다)
    /// </summary>
    /// <param name="gameObject">사용할 오브젝트</param>
    /// <param name="prefabType">오브젝트 타입</param>
    /// <param name="spawnPosition">오브젝트 생성 위치</param>
    /// <param name="quaternion">오브젝트 회전 위치</param>
    /// <returns>Queue에서 반환된 SetActive(false)오브젝트</returns>
    public GameObject OutputGameObject(GameObject gameObject, string perfabPath, Vector2 spawnPosition, Quaternion quaternion)
    {
        GameObject temp;

        // 해당 오브젝트 전용 큐가 없으면.
        if (!pools.TryGetValue(gameObject.name, out _))
        {
            // 해당 오브젝트 전용 큐를 만든다.
            pools.Add(gameObject.name, new Queue<GameObject>());
         //   Debug.Log($"{gameObject.name}전용 큐 생성");
        }

        // 오브젝트 전용 큐가 비웠으면
        if (pools[gameObject.name].Count <= 0)
        {
            // 즉석으로 오브젝트를 동적생성하여 큐에 넣는다.
            temp = Instantiate(Managers.Resource.GetPerfabGameObject(perfabPath));
            temp.name = gameObject.name; // 프리팹 (Clone) 이름 삭제
            temp.SetActive(false);
            //  pools[gameObject.name].Enqueue(temp);             // -> 각 오브젝트.cs OnDisable()에서 Enqueue 진행
         //   Debug.Log($"{gameObject.name}전용 큐가 비워서 EnQueue");
        }


        // 큐에 있는 오브젝트를 꺼내온다.
        temp = pools[gameObject.name].Dequeue();
        // 오브젝트 생성될 위치와 회전값 설정
        temp.transform.SetPositionAndRotation(spawnPosition, quaternion);   

    //    Debug.Log($"{gameObject.name}전용 큐 DeQueue");
    //    print(temp.gameObject.name);
        return temp;
    }
    public void InitPool()
    {
        pools.Clear();
    }

    private void OnEnable()
    {
        pools = new Dictionary<string, Queue<GameObject>>();
    }
}
