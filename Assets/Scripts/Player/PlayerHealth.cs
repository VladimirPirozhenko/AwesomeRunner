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
public class PlayerHealth : MonoBehaviour, IHealth, IResettable
{
    public int MaxLives { get; private set; }
    public int CurrentLives { get; private set; }
    public bool IsInvincible { get; private set; }
    public float InvincibilityTime { get; private set; } //PLAYER DATA ScriptableObject
    private HealthArgs healthArgs;
    public event EventHandler<HealthArgs> OnHealthChanged = delegate { };
    public event Action OnDied;

    void Awake()
    {
        MaxLives = 3; // PlayerData.MaxLives
        CurrentLives = MaxLives;
        InvincibilityTime = 3f; //PlayerData/InvincibilityTimer
        healthArgs = new HealthArgs(CurrentLives, MaxLives);
    }

    public void TakeDamage()
    {
        CurrentLives--;
        OnHealthChanged(this, healthArgs.GetUpdatedArgs(CurrentLives, MaxLives));
        if (CurrentLives < 1)
        {
            OnDied?.Invoke();
        }
    }
    public void RestoreHealth()
    {
        if (CurrentLives < MaxLives)
            CurrentLives++;
        OnHealthChanged(this, healthArgs.GetUpdatedArgs(CurrentLives, MaxLives));
    }
    public IEnumerator GrantInvincibility()
    {
        IsInvincible = true;
        yield return new WaitForSeconds(InvincibilityTime);
        IsInvincible = false;
    }

    public void ResetToDefault()
    {
        MaxLives = 3; // PlayerData.MaxLives
        CurrentLives = MaxLives;
        InvincibilityTime = 3f; //PlayerData/InvincibilityTimer
        OnHealthChanged(this, healthArgs.GetUpdatedArgs(CurrentLives, MaxLives));
    }
}
