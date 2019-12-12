using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : IHealth, IDamageable
{
    public event Action<float> OnDamageTaken;
    public event Action OnDeath;
    public float MaxHealth { get; private set; }
    public float Health { get; private set; }

    /// <summary>
    /// Deals damage and returns damage actually dealt
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public float TakeDamage(float amount)
    {
        var healthAfter = Mathf.Clamp(Health - amount, 0.0f, MaxHealth);
        var dmgDealt = Health - healthAfter;
        Health = healthAfter;
        OnDamageTaken?.Invoke(Health);

        if(Health <= 0.0f)
        {
            OnDeath?.Invoke();
        }

        return dmgDealt;
    }

    public void ResetHealth()
    {
        Health = MaxHealth;
    }
}
