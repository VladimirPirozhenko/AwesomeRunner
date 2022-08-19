using UnityEngine;

public class Turret : PoolingObject<Turret>, IObstacle, IDamageDealer, IResettable
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