using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamage : IDisposable
{
    public float Damage
    {
        get
        {
            return Modifiers.CalculateFinalValue(_settings.baseDamage);
        }
    }
    public ModifierList Modifiers { get; } = new ModifierList();

    private IProjectileHit _hit;
    private Settings _settings;

    public ProjectileDamage(IProjectileHit collision, Settings settings)
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

    private void OnHit(IDamageable health)
    {
        health.TakeDamage(Damage);
    }


    [System.Serializable]
    public class Settings
    {
        public float baseDamage;
    }
}
