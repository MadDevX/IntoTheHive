using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    /// <summary>
    /// Deals damage to damageable object and returns damage actually dealt.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    float TakeDamage(float amount);
    /// <summary>
    /// 
    /// </summary>
    event Action<DamageTakenArgs> OnDamageTaken;
    event Action<DeathParameters> OnDeath;
}

public struct DamageTakenArgs
{
    public float damage;
    public float remainingHealth;

    public DamageTakenArgs(float damage, float remainingHealth)
    {
        this.damage = damage;
        this.remainingHealth = remainingHealth;
    }
}
