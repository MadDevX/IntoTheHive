﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterHealth : IHealth, IDamageable, IDisposable
{
    public event Action<float> OnDamageTaken;
    public event Action OnDeath;
    public float MaxHealth { get; private set; }
    public float Health { get; private set; }

    private Settings _settings;
    private IRespawnable _respawnable;

    public CharacterHealth(Settings settings, IRespawnable respawnable)
    {
        _settings = settings;
        _respawnable = respawnable;
        PreInitialize();
    }

    public void PreInitialize()
    {
        MaxHealth = _settings.maxHealth;
        _respawnable.OnSpawn += OnSpawned;
    }
    public void Dispose()
    {
        _respawnable.OnSpawn -= OnSpawned;
    }

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
        Debug.Log($"{dmgDealt} DMG taken! {Health} HP remaining");
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

    private void OnSpawned(CharacterSpawnParameters parameters)
    {
        //TODO: read health from params
        ResetHealth();
    }

    [System.Serializable]
    public class Settings
    {
        public float maxHealth;
    }
}
