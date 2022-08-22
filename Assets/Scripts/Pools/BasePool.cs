using UnityEngine;

public interface IBasePool
{

}

public abstract class BasePool<T> : MonoBehaviour where T : PoolingObject<T>
{
    [SerializeField] private int capacity;
    [SerializeField] private T prefab; 
    public int Capacity { get { return pool.Capacity; } private set { capacity = value; } }
    public int InitialCapacity { get; private set; }

    private ObjectPool<T> pool;

    private void Awake()
    {
        pool = new ObjectPool<T>(CreateAction, GetAction, ReturnAction, DestroyAction, capacity);
        InitialCapacity = capacity;
    }

    #region Actions
    protected virtual T CreateAction()
    {
        T instance = Instantiate(prefab);
        instance.transform.SetParent(this.transform, false);
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
    #endregion

    #region CountElements
    public int GetActiveElementCount()
    {
        return pool.GetActiveElementsCount();
    }

    public int GetInactiveElementCount()
    {
        return pool.GetInactiveElementsCount();
    }
    #endregion

    #region ContiansCheck
    public bool ContainsElement(T element)
    {
        return pool.ContainsElement(element);
    }
   
    public bool ContainsElement(T element, bool isActive)
    {
        return pool.ContainsElement(element, isActive);
    }
    #endregion

    #region GetFromPool
    public T Spawn()
    {
        return pool.Get();
    }
    public T Spawn(Vector3 position)
    {
        T obj = pool.Get();
        obj.transform.position = position;
        return obj;
    }
    public bool TryGetFromPos(in Vector3 pos, out T element)
    {
        if (pool.TryGetFromPos(pos, out element))
        {
            return true;
        }
        return false;
    }

    public Component GetComponentFromPool<C>() where C : Component
    {
        return pool.GetComponentFromPool<C>();
    }

    public C GetComponentFromPool<C>(T obj) where C : Component
    {
        return pool.GetComponentFromPool<C>(obj);  
    }

    #endregion

    #region ReturnToPool
    public void ReturnAllElementsToPool()
    {
        pool.ReturnAllElementsToPool();
    }

    public void ReturnToPool(T instance)
    {
        pool.ReturnToPool(instance);
    }
    #endregion

    #region DestroyFromPool
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
    #endregion
}

