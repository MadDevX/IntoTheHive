using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHit : IProjectileHit, IDisposable
{
    public event Action<IDamageable> OnHit;

    private IProjectileCollision _collision;
    private IProjectileDummy _dummy;

    public ProjectileHit(IProjectileCollision collision, IProjectileDummy dummy)
    {
        _collision = collision;
        _dummy = dummy;
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
        if (_dummy.IsDummy == false)
        {
            var health = obj.GetComponent<IDamageable>();

            if (health != null)
            {
                OnHit?.Invoke(health);
            }
        }
    }
}
