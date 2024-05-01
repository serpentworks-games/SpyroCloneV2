using System.Collections.Generic;
using UnityEngine;

public class ObjectPool {

    PoolableObject obj;
    List<PoolableObject> availableObjects;

    private ObjectPool(PoolableObject obj, int poolSize)
    {
        this.obj = obj;
        availableObjects = new List<PoolableObject>(poolSize);
    }

    public static ObjectPool CreateInstance(PoolableObject obj, int poolSize)
    {
        ObjectPool pool = new ObjectPool(obj, poolSize);

        GameObject poolObj = new GameObject($"{obj.name} Pool");
        pool.CreateObjects(poolObj.transform, poolSize);

        return pool;
    }

    private void CreateObjects(Transform parent, int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            PoolableObject poolableObj = GameObject.Instantiate(obj, Vector3.zero, Quaternion.identity, parent.transform);
            poolableObj.parentObj = this;
            poolableObj.gameObject.SetActive(false);
        }
    }

    public void ReturnObjectToPool(PoolableObject obj)
    {
        availableObjects.Add(obj);
    }

    public PoolableObject GetObject()
    {
        if (availableObjects.Count == 0)
        {
            return null;
        }
        else if (availableObjects.Count > 0)
        {
            //grab the first object in the pool
            PoolableObject instance = availableObjects[0];
            availableObjects.RemoveAt(0);

            instance.gameObject.SetActive(true);
            return instance;
        }
        return null;
    }
}