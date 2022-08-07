using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Передавать Action по добавлению и удалению из пула
public class ObjectPool<T> : IEnumerable<T> where T : MonoBehaviour
{
    public int Capacity { get; private set; }
    private Func<T> actionOnCreate;
    private Action<T> actionOnGet;
    private Action<T> actionOnRelease;

    private List<T> pool;
    private List<T> inactiveElements;
    public ObjectPool(Func<T> actionOnCreate, Action<T> actionOnGet, Action<T> actionOnRelease, int initialCapacity)
    {
        Capacity = initialCapacity;
        this.actionOnCreate = actionOnCreate;
        
        this.actionOnGet = actionOnGet;
        this.actionOnRelease = actionOnRelease;
        pool = new List<T>();
        for (uint i = 0; i < Capacity; i++)
        {
            var obj = actionOnCreate();
            pool.Add(obj);
        }
    }
    public T this[int i]
    {
        get => pool[i];
        set => pool[i] = value;
    }
    public T TryGetFromPos(in Vector3 pos,bool isActive)
    {

        foreach (var obj in pool)
        {
            if (isActive)
            {
                if (obj.gameObject.activeInHierarchy && pos == obj.gameObject.transform.position)
                {
                    actionOnGet.Invoke(obj);
                    return obj;
                }
            }
            else
            {
                if (!obj.gameObject.activeInHierarchy && pos == obj.gameObject.transform.position)
                {
                    actionOnGet.Invoke(obj);
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
    private bool TryGet(out T element)
    {
        List<T> inactiveObjects = GetInactiveElements();
        if (inactiveObjects.Count > 0)
        {
            var obj = inactiveObjects[UnityEngine.Random.Range(0, inactiveObjects.Count)];
            element = obj;
            actionOnGet.Invoke(element);
            return true;
        }
        element = null;
        return false;
    }
    public List<T> GetInactiveElements()
    {
        return pool.FindAll(obj => !obj.gameObject.activeInHierarchy);
    }
    public List<T> GetActiveElements()
    {
        return pool.FindAll(obj => obj.gameObject.activeInHierarchy);
    }
    public T ExpandPool()
    {
        var obj = actionOnCreate();
        Capacity++;
        pool.Add(obj);
        return obj;
    }
    public void ReturnToPool(T obj)
    {
        if (obj == null)
            return;
        if (obj.gameObject.activeInHierarchy)
        {
            actionOnRelease.Invoke(obj);
        }
        return;
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

