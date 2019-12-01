using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidProjectileVFX : IDisposable
{
    private readonly TrailRenderer _trail;
    private readonly ProjectileInitializer _initializer;

    public RigidProjectileVFX(TrailRenderer trail, ProjectileInitializer initializer)
    {
        _trail = trail;
        _initializer = initializer;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _initializer.OnProjectileInitialized += ClearTrail;
    }

    public void Dispose()
    {
        _initializer.OnProjectileInitialized -= ClearTrail;
    }

    private void ClearTrail()
    {
        _trail.Clear();
    }
}
