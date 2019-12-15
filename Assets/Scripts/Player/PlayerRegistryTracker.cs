using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRegistryTracker : IDisposable
{
    private IRespawnable _respawnable;
    private PlayerRegistry _registry;
    private CharacterFacade _facade;


    public PlayerRegistryTracker(IRespawnable respawnable, PlayerRegistry registry, CharacterFacade facade)
    {
        _respawnable = respawnable;
        _registry = registry;
        _facade = facade;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _respawnable.OnSpawn += OnSpawned;
        _respawnable.OnDespawn += OnDespawned;
    }

    public void Dispose()
    {
        _respawnable.OnSpawn -= OnSpawned;
        _respawnable.OnDespawn -= OnDespawned;
    }

    private void OnSpawned(CharacterSpawnParameters obj)
    {
        _registry.Player = _facade;
    }

    private void OnDespawned()
    {
        _registry.Player = null;
    }


}
