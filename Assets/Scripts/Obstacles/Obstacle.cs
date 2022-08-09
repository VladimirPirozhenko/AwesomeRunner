using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IObstacle
{
	public void Impact();
}
[RequireComponent(typeof(BoxCollider))]

public class Obstacle : MonoBehaviour,IObstacle,IDamageDealer,IResettable,IPoolable<Obstacle>
{
    public BoxCollider Collider { get; private set; }
    public BasePool<Obstacle> OwningPool { private get;  set; }

    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
    }
    public void ResetToDefault()
    {   
        transform.localPosition = Vector3.zero;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        gameObject.transform.SetParent(OwningPool.transform);
        ReturnToPool();
    }
    public void Impact()
    {
        ResetToDefault();
    }

    public void DealDamage(IDamageable target, int amount)
    {
        target.TakeDamage(amount);
    }

    public void ReturnToPool()
    {
        OwningPool.ReturnToPool(this);
    }
}

public class Turret : MonoBehaviour, IObstacle, IDamageDealer, IResettable
{
    public void DealDamage(IDamageable target, int amount)
    {
        target.TakeDamage(amount);
    }

    public void Impact()
    {
        gameObject.SetActive(true);
    }

    public void ResetToDefault()
    {
        gameObject.SetActive(true);
    }
}