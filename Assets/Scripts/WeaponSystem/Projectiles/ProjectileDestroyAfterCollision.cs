﻿using System;
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

    public void PreInitialize()
    {
        _collision.OnCollisionEnter += OnCollisionEnter;
        _initializer.OnProjectileInitialized += ResetCollisions;
    }

    public void Dispose()
    {
        _collision.OnCollisionEnter -= OnCollisionEnter;
        _initializer.OnProjectileInitialized -= ResetCollisions;
    }

    private void ResetCollisions()
    {
        _remainingCollisions = CollisionLimit;
    }

    private void OnCollisionEnter(Collider2D collider)
    {
        _remainingCollisions--;
        if(_remainingCollisions < 0)
        {
            _facade.Destroy();
        }
    }
}
