using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ObjectPoolingData
{
    public int objectKey;
    public GameObject poolGameObjectPrefab;
    public int ammount;
}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;
    public Dictionary<int, Queue<GameObject>> objects;
    public List<ObjectPoolingData> ObjectsToPool = new List<ObjectPoolingData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        SpawnPool();
    }

    // Start is called before the first frame update
    void SpawnPool()
    {
        objects = new Dictionary<int, Queue<GameObject>>();

        for (int i = 0; i < ObjectsToPool.Count; i++)
        {
            Queue<GameObject> pool = new Queue<GameObject>();

            for (int x = 0; x < ObjectsToPool[i].ammount; x++)
            {
                GameObject obj = Instantiate(ObjectsToPool[i].poolGameObjectPrefab, transform.position, Quaternion.identity);

                obj.transform.SetParent(transform);
                obj.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
                obj.SetActive(false);

                pool.Enqueue(obj);
            }

            objects.Add(ObjectsToPool[i].objectKey, pool);
        }
    }

    public GameObject GetObjectFromPool(int poolKey)
    {
        GameObject objectToGet = objects[poolKey].Dequeue();
        objectToGet.SetActive(true);
        objects[poolKey].Enqueue(objectToGet);
        return objectToGet;
    }

    public void SetObjectToPoolerParent(Transform obj)
    {
        obj.SetParent(transform);
    }
}
