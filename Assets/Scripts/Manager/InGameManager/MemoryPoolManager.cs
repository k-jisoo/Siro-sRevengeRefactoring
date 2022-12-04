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
    /// ������Ʈ �ݳ�(�ִ´�)
    /// </summary>
    /// <param name="gameObject">�ݳ� SetActive(ture)������Ʈ</param>
    public void InputGameObject(GameObject gameObject)
    {
        // �ݳ��� Queue�� ������ �״�� ���� X
        if (!pools.ContainsKey(gameObject.name)) return;

      //  print(gameObject.name + " �ݳ�");
        gameObject.SetActive(false);
        pools[gameObject.name].Enqueue(gameObject);
    }

    /// <summary>
    /// ������Ʈ ���(������)
    /// </summary>
    /// <param name="gameObject">����� ������Ʈ</param>
    /// <param name="prefabType">������Ʈ Ÿ��</param>
    /// <param name="spawnPosition">������Ʈ ���� ��ġ</param>
    /// <param name="quaternion">������Ʈ ȸ�� ��ġ</param>
    /// <returns>Queue���� ��ȯ�� SetActive(false)������Ʈ</returns>
    public GameObject OutputGameObject(GameObject gameObject, string perfabPath, Vector2 spawnPosition, Quaternion quaternion)
    {
        GameObject temp;

        // �ش� ������Ʈ ���� ť�� ������.
        if (!pools.TryGetValue(gameObject.name, out _))
        {
            // �ش� ������Ʈ ���� ť�� �����.
            pools.Add(gameObject.name, new Queue<GameObject>());
         //   Debug.Log($"{gameObject.name}���� ť ����");
        }

        // ������Ʈ ���� ť�� �������
        if (pools[gameObject.name].Count <= 0)
        {
            // �Ｎ���� ������Ʈ�� ���������Ͽ� ť�� �ִ´�.
            temp = Instantiate(Managers.Resource.GetPerfabGameObject(perfabPath));
            temp.name = gameObject.name; // ������ (Clone) �̸� ����
            temp.SetActive(false);
            //  pools[gameObject.name].Enqueue(temp);             // -> �� ������Ʈ.cs OnDisable()���� Enqueue ����
         //   Debug.Log($"{gameObject.name}���� ť�� ����� EnQueue");
        }


        // ť�� �ִ� ������Ʈ�� �����´�.
        temp = pools[gameObject.name].Dequeue();
        // ������Ʈ ������ ��ġ�� ȸ���� ����
        temp.transform.SetPositionAndRotation(spawnPosition, quaternion);   

    //    Debug.Log($"{gameObject.name}���� ť DeQueue");
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
