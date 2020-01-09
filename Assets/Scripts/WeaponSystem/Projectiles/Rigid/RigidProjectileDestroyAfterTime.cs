using GameLoop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidProjectileDestroyAfterTime : IDisposable
{
    private IProjectileFixedTime _fixedTime;
    private ProjectileInitializer _initializer;
    private IProjectile _projectile;
    private float _ttl;

    public RigidProjectileDestroyAfterTime(IProjectileFixedTime fixedTime, ProjectileInitializer initializer, IProjectile projectile)
    {
        _fixedTime = fixedTime;
        _initializer = initializer;
        _projectile = projectile;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _fixedTime.OnFixedUpdateEvt += OnFixedUpdate;
        _initializer.OnProjectileCreated += OnCreated;
    }

    public void Dispose()
    {
        _fixedTime.OnFixedUpdateEvt -= OnFixedUpdate;
        _initializer.OnProjectileCreated -= OnCreated;
    }

    private void OnCreated(ProjectileSpawnParameters obj)
    {
        _ttl = obj.ttl;
    }

    private void OnFixedUpdate(float obj)
    {
        if(_fixedTime.FixedTravelTime >= _ttl)
        {
            _projectile.Destroy();
        }
    }
}
