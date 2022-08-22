using UnityEngine;

public abstract class PoolingObject<T> : MonoBehaviour where T : PoolingObject<T>
{
    public BasePool<T> OwningPool { protected get; set; }

    public C GetComponentFromPool<C>() where C : Component
    {
        return OwningPool.GetComponentFromPool<C>(this as T);
    }

    public void ReturnToPool()
    {
        OwningPool.ReturnToPool(this as T);
    }
}
