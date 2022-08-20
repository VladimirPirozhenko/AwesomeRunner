using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool<T> : IEnumerable<T> where T : MonoBehaviour
{
    public int Capacity { get; private set; }

    private Func<T> actionOnCreate;
    private Action<T> actionOnGet;
    private Action<T> actionOnRelease;
    private Action<T> actionOnDestroy;

    private List<T> activePoolElements;
    private Queue<T> inactivePoolElements;

    public ObjectPool(Func<T> actionOnCreate, Action<T> actionOnGet, Action<T> actionOnRelease, Action<T> actionOnDestroy, int initialCapacity,bool isActiveByDefault)
    {
        Capacity = initialCapacity;
        this.actionOnCreate = actionOnCreate;       
        this.actionOnGet = actionOnGet;
        this.actionOnRelease = actionOnRelease;
        this.actionOnDestroy = actionOnDestroy;
        activePoolElements = new List<T>();
        inactivePoolElements = new Queue<T>();

        for (uint i = 0; i < Capacity; i++)
        {
            var obj = actionOnCreate();
           // inactivePoolElements.Enqueue(obj);
            //obj.gameObject.SetActive(isActiveByDefault);
            if (isActiveByDefault)
            {
                actionOnGet(obj);
            }
            else
            {
                actionOnRelease(obj);
            }  
        }
    }

    public bool ContainsElement(T element)
    {
        return ContainsElement(element, true) || ContainsElement(element, false);
    }

    public bool ContainsElement(T element,bool isActive)
    {
        if (isActive)
        {
            return activePoolElements.Contains(element);
        }
        else
        {
            return inactivePoolElements.Contains(element);  
        } 
    }

    public bool TryGetFromPos(in Vector3 pos,out T element)
    {
        element = null;
        foreach (var obj in activePoolElements)
        {
            if (pos == obj.gameObject.transform.position)
            {
                actionOnGet.Invoke(obj);
                element = obj;
                return true;    
            }
        }  
        return false;
    }
    public T Get()
    {
        if (TryGet(out var element))
        {
            return element;
        }
        T instance = ExpandPool();
        return instance;
    }

    private T ExpandPool()
    {
        var obj = actionOnCreate();
        Capacity++;
        obj.gameObject.SetActive(true);
        activePoolElements.Add(obj);
        return obj;
    }

    private bool TryGet(out T element)
    {
        element = null;
        if (inactivePoolElements.Count > 0)
        {
            var obj = inactivePoolElements.Dequeue();
            if (obj == null)
            {
                return false;   
            }
            element = obj;
            actionOnGet.Invoke(element);
            activePoolElements.Add(obj);
            return true;
        }    
        return false;
    }

    public void Destroy(T obj)
    {
        activePoolElements.Remove(obj);
        actionOnDestroy.Invoke(obj);
    }
    public void DestroyAllElements()
    {
        DestroyAllElements(true);
        DestroyAllElements(false);
    }
    public void DestroyAllElements(bool isActive)
    {
        if (isActive)
        {
            foreach (var obj in activePoolElements)
            {
                Destroy(obj);
            }
            activePoolElements.Clear();
        }
        else
        {
            foreach (var obj in inactivePoolElements)
            {
                Destroy(obj);
            }
            inactivePoolElements.Clear();
        }  
    }

    public void ReturnAllElementsToPool()
    {
        foreach (var obj in activePoolElements)
        {
            ReturnToPool(obj);
        }
        activePoolElements.Clear();
    }
 
    public int GetInactiveElementsCount()
    {
        return inactivePoolElements.Count();
    }

    public int GetActiveElementsCount()
    {
        return activePoolElements.Count();
    }

    public void ReturnToPool(T obj)
    {
        if (obj == null)
        {
            activePoolElements.RemoveAll(o => o == null);
            return;
        }
        if (activePoolElements.Remove(obj))
        {
            actionOnRelease.Invoke(obj);
            inactivePoolElements.Enqueue(obj);
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return activePoolElements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

