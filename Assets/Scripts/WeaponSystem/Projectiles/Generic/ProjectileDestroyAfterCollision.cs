using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectileDestroyAfterCollision : IDisposable
{
    public int CollisionLimit { get; set; }

    private int _remainingCollisions;

    private IProjectileCollision _collision;
    private ProjectilePhasePipeline _pipeline;
    private ProjectileInitializer _initializer;

    private IProjectile _facade;

    public ProjectileDestroyAfterCollision(IProjectileCollision collision, 
        IProjectile facade,
        ProjectilePhasePipeline pipeline,
        ProjectileInitializer initializer)
    {
        _collision = collision;
        _pipeline = pipeline;
        _initializer = initializer;
        _facade = facade;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _collision.AfterCollisionEnter += AfterCollisionEnter;
        _initializer.OnProjectileDefined += ResetCollisions;
    }

    public void Dispose()
    {
        _collision.AfterCollisionEnter -= AfterCollisionEnter;
        _initializer.OnProjectileDefined -= ResetCollisions;
    }

    private void ResetCollisions()
    {
        _remainingCollisions = CollisionLimit;
    }

    private void AfterCollisionEnter()
    {
        _remainingCollisions--;
        if(_remainingCollisions < 0)
        {
            _facade.Destroy();
        }
    }
}
