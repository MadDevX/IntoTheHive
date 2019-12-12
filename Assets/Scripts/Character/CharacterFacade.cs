using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterFacade : MonoBehaviour, IDamageable
{
    private IHealth _health;

    [Inject]
    public void Construct(IHealth health)
    {
        _health = health;
    }

    public float TakeDamage(float amount)
    {
        return _health.TakeDamage(amount);
    }
}
