using System;
using UnityEngine;

public interface IBasePool
{
    public void Init();
    public void ReturnToPool<PoolingObject>(PoolingObject instance);
}
public class BasePool<T> : MonoBehaviour, IBasePool where T : PoolingObject
{
    [field: SerializeField] public int Capacity { get; private set; }
    [SerializeField] private T prefab;
    private ObjectPool<T> pool; 

    private void Awake()
    {
        pool = new ObjectPool<T>(CreateAction, GetAction, ReturnAction, Capacity);
    }
    public BasePool<PoolingObject> GetPool()
    {
        return (BasePool<PoolingObject>)Convert.ChangeType(this, typeof(BasePool<PoolingObject>)); 
    }
    protected virtual T CreateAction()
    {
        T instance = Instantiate(prefab);
        instance.gameObject.SetActive(false);
        instance.transform.SetParent(gameObject.transform, false);
        //instance.transform.SetParent(transform);
       // instance.Init(this);
       // BasePool<PoolingObject> pool = (BasePool<PoolingObject>)(object)this;
        instance.OwningPool  = this;
        return instance;
    }
    protected virtual void GetAction(T instance)
    {
        instance.gameObject.SetActive(true);
    }

    protected virtual void ReturnAction(T instance)
    {
        instance.transform.SetParent(transform);
        instance.gameObject.SetActive(false);
    }

    public T GetFromPool()
    {
        return pool.Get();
    }

    public void ReturnToPool<PoolingObject>(PoolingObject instance)
    {
        pool.ReturnToPool(instance as T);
    }

    public void Init()
    {
        //instance.transform.SetParent(transform);
    }

    //public void ReturnToPool(T instance)
    //{
    //    pool.ReturnToPool(instance);
    //}

    //public void ReturnToPool<T>(T instance)
    //{
    //    pool.ReturnToPool(instance);
    //}

    //public static implicit operator BasePool<T>(BasePool<PoolingObject> v)
    //{
    //    return v;   
    //}
}

