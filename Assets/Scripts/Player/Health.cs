using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthArgs : EventArgs
{
    public int MaxLives { get; private set; }
    public int CurrentLives { get; private set; }

    public HealthArgs(int currentLives,int maxLives)
    {
        CurrentLives = currentLives;
        MaxLives = maxLives;
    }
    public HealthArgs GetUpdatedArgs(int currentLives, int maxLives) //to avoid GC overhead
    {
        CurrentLives = currentLives;
        MaxLives = maxLives;
        return this;
    }
}
public class Health : MonoBehaviour, IDamageable,IHealable, IResettable
{
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    [SerializeField] private HeartsBarView healthBarView;

    private HealthArgs healthArgs;
    public event EventHandler<HealthArgs> OnHealthChanged = delegate { };
    public event Action OnOutOfHealth;

    private int defaultMaxLives = 4;//PLAYER DATA ScriptableObject
    void Awake()
    {
        MaxHealth = 4;
        healthArgs = new HealthArgs(CurrentHealth, MaxHealth);
        healthBarView.CreateHealthBar(healthArgs);
        healthBarView.Show(true);
        ResetToDefault();
    } 
    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        OnHealthChanged(this, healthArgs.GetUpdatedArgs(CurrentHealth, MaxHealth));
        healthBarView.UpdateHealthBar(this, healthArgs.GetUpdatedArgs(CurrentHealth, MaxHealth));
        if (CurrentHealth < 1)
        {
            OnOutOfHealth?.Invoke();
        }
    }
    public void Heal(int amount)
    {
        if (CurrentHealth < MaxHealth)
            CurrentHealth += amount;
        OnHealthChanged(this, healthArgs.GetUpdatedArgs(CurrentHealth, MaxHealth));
        healthBarView.UpdateHealthBar(this, healthArgs.GetUpdatedArgs(CurrentHealth, MaxHealth));
    }
    public void ResetToDefault()
    {
        MaxHealth = defaultMaxLives; // PlayerData.MaxHealth
        CurrentHealth = MaxHealth;
        OnHealthChanged(this, healthArgs.GetUpdatedArgs(CurrentHealth, MaxHealth));
        healthBarView.UpdateHealthBar(this, healthArgs.GetUpdatedArgs(CurrentHealth, MaxHealth));
    }
}
