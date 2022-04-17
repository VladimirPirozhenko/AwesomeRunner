using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectPool<T> : IEnumerable<T> where T : MonoBehaviour
{
    [SerializeField] public int Capacity { get; private set; }
    public T First 
    {
        get
        {
            return pool.First.Value;
        }
    }
    public T Last
    {
        get
        {
            return pool.Last.Value;
        }
    }  
    private List<T> prefabs;
    private LinkedList<T> pool;
    public void FirstToLast()
    {
        T first = First;
        pool.RemoveFirst();
        pool.AddLast(first);
    }
    
    public ObjectPool(in List<T> prefabs, int initialCapacity, bool activeByDefault = false)
    {
        Capacity = initialCapacity;
        pool = new LinkedList<T>();
        for (uint i = 0; i < Capacity; i++)
        {
            var obj = createObject(prefabs[UnityEngine.Random.Range(0, prefabs.Count)], activeByDefault);
            pool.AddLast(obj);
        }
    }

    private T createObject(T prefab, bool activeByDefault = false)
    {
        var instance = UnityEngine.Object.Instantiate(prefab, new Vector3(), new Quaternion());
        instance.gameObject.SetActive(activeByDefault);
        return instance;
    }

    private bool TryGet(out T element)
    {
        if (pool.Count > 0)
        {
            foreach (var obj in pool)
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    element = obj;
                    element.gameObject.SetActive(true);
                    return true;
                }
            }
        }
        element = null;
        return false;
    }
    public T TryGetFromPos(Vector3 pos)
    {
        foreach (var obj in pool)
        {
            if (obj.gameObject.activeInHierarchy)
            {
                if (pos == obj.gameObject.transform.position)
                {
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }
        }
        return null;
    }
    public T Get()
    {
        if (TryGet(out var element))
        {
            return element;
        }
        return ExpandPool();
    }
    public T ExpandPool()
    {
        var obj = createObject(prefabs[UnityEngine.Random.Range(0, pool.Count)], true);
        Capacity++;
        pool.AddLast(obj);
        return obj;
    }
    public bool ReturnToPool(T obj)
    {
        if (obj.gameObject.activeInHierarchy)
        {
            obj.gameObject.SetActive(false);
            return true;
        }
        return false;
    }
    public IEnumerator<T> GetEnumerator()
    {
        return pool.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

