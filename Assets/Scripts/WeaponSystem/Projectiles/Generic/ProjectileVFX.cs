using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectileVFX : IDisposable
{
    private ProjectileInitializer _initializer;
    private ExplosionVFX.Factory _factory;
    private IProjectilePosition _position;

    public ProjectileVFX(
        ProjectileInitializer initializer, 
        [Inject(Id = Identifiers.Explosion)] ExplosionVFX.Factory factory, 
        IProjectilePosition position)
    {
        _initializer = initializer;
        _factory = factory;
        _position = position;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _initializer.OnProjectileDestroyed += OnDestroyed;
    }

    public void Dispose()
    {
        _initializer.OnProjectileDestroyed -= OnDestroyed;
    }

    private void OnDestroyed(ProjectileSpawnParameters obj)
    {
        _factory.Create(_position.Position);
    }
}
