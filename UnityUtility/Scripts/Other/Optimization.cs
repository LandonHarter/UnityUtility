using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimization : MonoBehaviour
{

    private static Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    private static List<Pool> pools = new List<Pool>();

    public static void CreatePool(Pool pool)
    {
        GameObject poolsGO;
        if (GameObject.Find("Optimization Pools") == null)
        {
            GameObject go = new GameObject();
            go.name = "Optimization Pools";
            poolsGO = go;
        }
        else
        {
            poolsGO = GameObject.Find("Optimization Pools");
        }

        GameObject parentObject = new GameObject();
        parentObject.name = pool.name;
        parentObject.transform.SetParent(poolsGO.transform);

        Queue<GameObject> objectPool = new Queue<GameObject>();
        pools.Add(pool);
        
        for (int i = 0; i < pool.size; i++)
        {
            GameObject obj = Instantiate(pool.Object);
            obj.SetActive(false);
            obj.transform.SetParent(parentObject.transform);
            objectPool.Enqueue(obj);
        }

        poolDictionary.Add(pool.name, objectPool);
    }

    public static GameObject GrabFromPool(string name, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion())
    {
        if (!poolDictionary.ContainsKey(name))
        {
            Debug.LogWarning("Object pool " + name + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[name].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetParent(null);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        if (objectToSpawn.GetComponent<PooledObject>() != null) objectToSpawn.GetComponent<PooledObject>().OnSpawned();

        poolDictionary[name].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

}

[System.Serializable]
public class Pool
{

    public string name;
    public GameObject Object;
    public int size;

    public Pool(string name, GameObject Object, int size)
    {
        this.name = name;
        this.Object = Object;
        this.size = size;
    }

}

public interface PooledObject
{

    void OnSpawned();

}