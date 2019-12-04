using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidProjectilePipelineManager : IDisposable
{
    private ProjectilePhasePipeline _pipeline;
    private ProjectileInitializer _initializer;
    private IProjectile _facade;
    private ProjectilePipelineParameters _parameters;

    public RigidProjectilePipelineManager(ProjectilePhasePipeline pipeline, ProjectileInitializer initializer, IProjectile facade)
    {
        _pipeline = pipeline;
        _initializer = initializer;
        _facade = facade;
        _parameters = new ProjectilePipelineParameters(_facade);
        PreInitialize();
    }

    private void PreInitialize()
    {
        _initializer.OnProjectileInitialized += OnProjectileInitialized;
        _initializer.OnProjectileDestroyed += OnProjectileDestroyed;
    }

    public void Dispose()
    {
        _initializer.OnProjectileInitialized -= OnProjectileInitialized;
        _initializer.OnProjectileDestroyed -= OnProjectileDestroyed;
    }

    private void OnProjectileInitialized()
    {
        _pipeline.SetState(ProjectilePhases.Created, _parameters);
    }

    private void OnProjectileDestroyed()
    {
        _pipeline.SetState(ProjectilePhases.Destroyed, _parameters);
    }
}
