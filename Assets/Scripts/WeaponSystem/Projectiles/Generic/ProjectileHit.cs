using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HitParameters
{
    public IDamageable damageable;
    public Transform transform;

    public HitParameters(IDamageable damageable, Transform transform)
    {
        this.damageable = damageable;
        this.transform = transform;
    }
}

public class ProjectileHit : IProjectileHit, IDisposable
{
    public event Action<HitParameters> OnHit;

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
            var damageable = obj.GetComponent<IDamageable>();

            if (damageable != null)
            {
                OnHit?.Invoke(new HitParameters(damageable, obj.transform));
            }
        }
    }
}
