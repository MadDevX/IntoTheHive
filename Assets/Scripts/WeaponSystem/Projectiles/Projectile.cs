using GameLoop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public struct ProjectileSpawnParameters
{
    public Vector2 position;
    public float rotation;
    public float velocity;
    public float ttl;

    public ProjectileSpawnParameters(Vector2 position, float rotation, float velocity, float ttl)
    {
        this.position = position;
        this.rotation = rotation;
        this.velocity = velocity;
        this.ttl = ttl;
    }
}

public class Projectile : MonoUpdatableObject, IPoolable<ProjectileSpawnParameters, IMemoryPool>, IDisposable
{
    [SerializeField] private ProjectilePhysics _rb;

    private IMemoryPool _pool;
    private float _ttl = 1.0f;

    public void Dispose()
    {
        _pool.Despawn(this);
    }

    public void OnDespawned()
    {
        _pool = null;
        _rb.Velocity = Vector2.zero;
    }

    public void OnSpawned(ProjectileSpawnParameters parameters, IMemoryPool pool)
    {
        _rb.Position = parameters.position;
        _rb.Velocity = Vector2.up.Rotate(parameters.rotation) * parameters.velocity;
        _ttl = parameters.ttl;
        _pool = pool;
    }

    public override void OnUpdate(float deltaTime)
    {
        _ttl -= deltaTime;
        if (_ttl < 0.0f)
        {
            Dispose();
        }
    }

    public class Factory : PlaceholderFactory<ProjectileSpawnParameters, Projectile>
    {
    }
}
