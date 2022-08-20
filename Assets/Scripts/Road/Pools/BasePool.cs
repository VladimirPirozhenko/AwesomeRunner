using UnityEngine;

public class BasePool<T> : MonoBehaviour where T : PoolingObject<T>
{
    [SerializeField] private int capacity;
    [SerializeField] private bool isActiveByDefault;
    [SerializeField] private T prefab; 
    public int Capacity { get { return pool.Capacity; } private set { capacity = value; } }
    public int InitialCapacity { get; private set; }

    private ObjectPool<T> pool; 

    private void Awake()
    {
        pool = new ObjectPool<T>(CreateAction, GetAction, ReturnAction, DestroyAction,capacity,isActiveByDefault);
        InitialCapacity = capacity; 
    }

    protected virtual T CreateAction()
    {
        T instance = Instantiate(prefab);
        instance.transform.SetParent(gameObject.transform, false);
        instance.OwningPool = this;
        return instance;
    }

    protected virtual void GetAction(T instance)
    {
        instance.gameObject.SetActive(true);
    }

    protected virtual void ReturnAction(T instance)
    {
        instance.gameObject.SetActive(false);
    }
    protected virtual void DestroyAction(T instance)
    {
        Destroy(instance.gameObject);
    }

    public int GetActiveElementCount()
    {
        return pool.GetActiveElementsCount();
    }

    public int GetInactiveElementCount()
    {
        return pool.GetInactiveElementsCount();
    }

    public bool ContainsElement(T element)
    {
        return pool.ContainsElement(element);
    }

    public bool ContainsElement(T element, bool isActive)
    {
        return pool.ContainsElement(element, isActive);
    }

    public bool TryGetFromPos(in Vector3 pos, out T element)
    {
        if (pool.TryGetFromPos(pos, out element))
        {
            return true;
        }
        return false;
    }

    public void Destroy(T obj)
    {
        pool.Destroy(obj);
    }

    public void DestroyAllElements()
    {
        pool.DestroyAllElements();  
    }

    public void DestroyAllElements(bool isActive)
    {
        pool.DestroyAllElements(isActive);  
    }

    public void ReturnAllElementsToPool()
    {
        pool.ReturnAllElementsToPool();    
    }

    public T Spawn()
    {
        return pool.Get();
    }

    public void ReturnToPool(T instance)
    {
        pool.ReturnToPool(instance);
    }
}

