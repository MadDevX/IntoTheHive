using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayProjectileLocation : IDisposable, IProjectilePosition, IProjectileVelocity
{
    private ProjectileInitializer _initializer;
    public Vector2 Velocity { get; set; }
    public Vector2 Position { get; set; }

    public RayProjectileLocation(ProjectileInitializer initializer)
    {
        _initializer = initializer;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _initializer.OnProjectileCreated += OnParticleCreated;
    }

    public void Dispose()
    {
        _initializer.OnProjectileCreated -= OnParticleCreated;
    }

    private void OnParticleCreated(ProjectileSpawnParameters parameters)
    {
        Position = parameters.position;
        Velocity = Vector2.up.Rotate(parameters.rotation) * parameters.velocity;
    }

}
