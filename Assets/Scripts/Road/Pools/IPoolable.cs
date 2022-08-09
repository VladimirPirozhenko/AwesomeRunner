
using UnityEngine;

public interface IPoolable<T> where T : MonoBehaviour,IPoolable<T>
{
    public BasePool<T> OwningPool { set; }
    public void ReturnToPool(); 
}
