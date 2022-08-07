using System;
public interface IDamageable
{
    public void TakeDamage(int amount);
    public event Action OnOutOfHealth;
}
