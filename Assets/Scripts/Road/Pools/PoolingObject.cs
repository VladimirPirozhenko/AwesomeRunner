using UnityEngine;
public interface IPoolable
{
    public void ReturnToPool();
}
public abstract class PoolingObject : MonoBehaviour, IPoolable //where T : PoolingObject<IPoolable>
{
    public IBasePool OwningPool { protected get; set; }

    public void Init()
    {
        OwningPool.Init();
    }

    public void ReturnToPool()
    {
        OwningPool.ReturnToPool(this);
    }
}
