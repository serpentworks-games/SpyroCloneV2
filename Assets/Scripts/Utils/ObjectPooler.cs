using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScalePact.Utils
{
    public class ObjectPooler<T> where T : MonoBehaviour, IPooled<T>
    {
        public T[] instances;

        protected Stack<int> freeIndex;

        public void Init(int count, T prefab)
        {
            instances = new T[count];
            freeIndex = new Stack<int>(count);

            for (int i = 0; i < count; i++)
            {
                instances[i] = Object.Instantiate(prefab);
                instances[i].gameObject.SetActive(false);
                instances[i].poolID = i;
                instances[i].pooler = this;

                freeIndex.Push(i);
            }
        }

        public T GetNewInstance()
        {
            int idx = freeIndex.Pop();
            instances[idx].gameObject.SetActive(true);

            return instances[idx];
        }

        public void FreeInstance(T obj)
        {
            freeIndex.Push(obj.poolID);
            instances[obj.poolID].gameObject.SetActive(false);
        }
    }

    public interface IPooled<T> where T : MonoBehaviour, IPooled<T>
    {
        int poolID { get; set; }
        ObjectPooler<T> pooler { get; set; }
    }
}