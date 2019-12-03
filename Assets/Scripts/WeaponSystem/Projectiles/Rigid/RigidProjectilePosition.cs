using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidProjectilePosition : IProjectilePosition, IDisposable
{
    public Vector2 Position { get => _rb.position; set => _rb.transform.position = value; }

    private Rigidbody2D _rb;
    private ProjectileInitializer _initializer;
    
    public RigidProjectilePosition(Rigidbody2D rb, ProjectileInitializer initializer)
    {
        _rb = rb;
        _initializer = initializer;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _initializer.OnProjectileCreated += SetPosition;
    }

    public void Dispose()
    {
        _initializer.OnProjectileCreated -= SetPosition;
    }

    private void SetPosition(ProjectileSpawnParameters parameters)
    {
        Position = parameters.position;
    }
}
