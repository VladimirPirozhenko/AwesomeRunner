using UnityEngine;

public abstract class PoolingObject<T> : MonoBehaviour where T : PoolingObject<T>
{
    public BasePool<T> OwningPool { protected get; set; }

    public void ReturnToPool()
    {
        OwningPool.ReturnToPool(this as T);
    }
}
