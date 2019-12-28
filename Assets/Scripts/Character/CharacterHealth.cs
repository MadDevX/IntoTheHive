using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterHealth : IHealth, IHealthSetter, IDisposable
{
    public float MaxHealth { get; private set; }
    public float Health { get; set; }

    private Settings _settings;
    private IRespawnable<CharacterSpawnParameters> _respawnable;

    public CharacterHealth(Settings settings, IRespawnable<CharacterSpawnParameters> respawnable)
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
