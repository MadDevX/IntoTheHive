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
    event Action<float> OnDamageTaken;
}
