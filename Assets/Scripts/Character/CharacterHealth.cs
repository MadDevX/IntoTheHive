using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterHealth : IHealth, IHealthSetter, IDisposable
{
    public float MaxHealth { get; private set; }
    public float Health { get; set; }

    private IHealthSettings _settings;
    private IRespawnable<CharacterSpawnParameters> _respawnable;

    public CharacterHealth(IHealthSettings settings, IRespawnable<CharacterSpawnParameters> respawnable)
    {
        _settings = settings;
        _respawnable = respawnable;
        PreInitialize();
    }

    public void PreInitialize()
    {
        MaxHealth = _settings.MaxHealth;
        _respawnable.OnSpawn += OnSpawned;
    }
    public void Dispose()
    {
        _respawnable.OnSpawn -= OnSpawned;
    }

    public void ResetHealth()
    {
        Health = MaxHealth;
    }

    private void OnSpawned(CharacterSpawnParameters parameters)
    {
        ResetHealth();
    }

    [System.Serializable]
    public class Settings : IHealthSettings
    {
        public float maxHealth;

        public float MaxHealth => maxHealth;
    }
}

public interface IHealthSettings
{
    float MaxHealth { get; }
}