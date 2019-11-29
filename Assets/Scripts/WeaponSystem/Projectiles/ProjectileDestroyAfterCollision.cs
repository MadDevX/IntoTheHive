using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectileDestroyAfterCollision : IInitializable, IDisposable
{
    public int CollisionLimit { get; set; }

    private int _remainingCollisions;

    private ProjectileCollisionHandler _colHandler;
    private ProjectilePhasePipeline _pipeline;
    private ProjectileInitializer _initializer;

    public ProjectileDestroyAfterCollision(ProjectileCollisionHandler colHandler, 
        ProjectilePhasePipeline pipeline,
        ProjectileInitializer initializer)
    {
        _colHandler = colHandler;
        _pipeline = pipeline;
        _initializer = initializer;
    }

    public void Initialize()
    {
        _colHandler.OnCollisionEnter += OnCollisionEnter;
        _initializer.OnProjectileInitialized += ResetCollisions;
    }

    public void Dispose()
    {
        _colHandler.OnCollisionEnter -= OnCollisionEnter;
        _initializer.OnProjectileInitialized -= ResetCollisions;
    }

    private void ResetCollisions()
    {
        _remainingCollisions = CollisionLimit;
    }

    private void OnCollisionEnter(IProjectile facade, Collider2D collider)
    {
        _remainingCollisions--;
        if(_remainingCollisions < 0)
        {
            _pipeline.SetState(ProjectilePhases.Destroy, new ProjectilePipelineParameters(facade));
        }
    }
}
