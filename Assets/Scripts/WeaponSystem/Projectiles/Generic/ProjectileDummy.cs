using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectileDummy : IDisposable, IProjectileDummy
{
    public bool IsDummy { get; private set; }
    private ProjectileInitializer _initializer;

    public ProjectileDummy(ProjectileInitializer initializer)
    {
        _initializer = initializer;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _initializer.OnProjectileCreated += OnCreated;
    }

    public void Dispose()
    {
        _initializer.OnProjectileCreated -= OnCreated;
    }

    private void OnCreated(ProjectileSpawnParameters obj)
    {
        IsDummy = obj.dummy;
    }
}
