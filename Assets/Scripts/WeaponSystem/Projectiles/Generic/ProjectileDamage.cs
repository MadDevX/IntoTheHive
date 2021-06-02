using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public struct DamageDealtParameters
{
    public HitParameters hitParameters;
    public float damage;

    public DamageDealtParameters(HitParameters hitParameters, float damage)
    {
        this.hitParameters = hitParameters;
        this.damage = damage;
    }
}

public class ProjectileDamage : IDisposable
{
    public event Action<DamageDealtParameters> OnDamageDealt;
    
    public float Damage
    {
        get
        {
            return Modifiers.CalculateFinalValue(_settings.baseDamage);
        }
    }
    public ModifierList Modifiers { get; } = new ModifierList();

    private IProjectileHit _hit;
    FloatingText.Factory _floatingTextFactory;
    private Settings _settings;

    public ProjectileDamage(
        IProjectileHit collision,
        Settings settings
        )
    {
        _hit = collision;
        _settings = settings;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _hit.OnHit += OnHit;
    }

    public void Dispose()
    {
        _hit.OnHit -= OnHit;
    }

    private void OnHit(HitParameters arguments)
    {
        var dealt = arguments.damageable.TakeDamage(Damage);
        OnDamageDealt?.Invoke(new DamageDealtParameters(arguments, dealt));
    }

    [System.Serializable]
    public class Settings
    {
        public float baseDamage;
    }
}
