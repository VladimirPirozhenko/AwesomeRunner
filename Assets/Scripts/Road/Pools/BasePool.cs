using UnityEngine;

public class BasePool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private int capacity;
    [SerializeField] private T prefab;
    private ObjectPool<T> pool; 

    private void Awake()
    {
        pool = new ObjectPool<T>(CreateAction, GetAction, ReturnAction, capacity);
    }

    protected virtual T CreateAction()
    {
        T instance = Instantiate(prefab);
        instance.gameObject.SetActive(false);
        instance.transform.SetParent(gameObject.transform, false);
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

    public T GetFromPool()
    {
        return pool.Get();
    }

    public void ReturnToPool(T instance)
    {
        pool.ReturnToPool(instance);
    }
}

