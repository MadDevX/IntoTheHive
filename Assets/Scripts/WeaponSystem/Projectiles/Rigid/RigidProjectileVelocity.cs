using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidProjectileVelocity : IProjectileVelocity, IDisposable
{
    public Vector2 Velocity { get => _rb.velocity; set => _rb.velocity = value; }

    private Rigidbody2D _rb;
    private ProjectileInitializer _initializer;

    public RigidProjectileVelocity(Rigidbody2D rb, ProjectileInitializer initializer)
    {
        _rb = rb;
        _initializer = initializer;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _initializer.OnProjectileCreated += SetVelocity;
    }

    public void Dispose()
    {
        _initializer.OnProjectileCreated -= SetVelocity;
    }

    private void SetVelocity(ProjectileSpawnParameters parameters)
    {
        Velocity = Vector2.up.Rotate(parameters.rotation) * parameters.velocity;
    }
}
