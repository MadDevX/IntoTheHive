using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHit : IProjectileHit, IDisposable
{
    public event Action<IDamageable> OnHit;

    private IProjectileCollision _collision;

    public ProjectileHit(IProjectileCollision collision)
    {
        _collision = collision;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _collision.OnCollisionEnter += OnCollisionEnter;
    }

    public void Dispose()
    {
        _collision.OnCollisionEnter -= OnCollisionEnter;
    }

    private void OnCollisionEnter(Collider2D obj)
    {
        var health = obj.GetComponent<IDamageable>();

        if (health != null)
        {
            OnHit?.Invoke(health);
        }
    }
}
