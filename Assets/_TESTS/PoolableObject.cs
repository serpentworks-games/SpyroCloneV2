using UnityEngine;

public class PoolableObject : MonoBehaviour {

    public ObjectPool parentObj;
    public virtual void OnDisable() 
    {
        parentObj.ReturnObjectToPool(this);
    }
}