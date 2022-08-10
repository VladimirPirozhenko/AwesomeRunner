using UnityEngine;

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