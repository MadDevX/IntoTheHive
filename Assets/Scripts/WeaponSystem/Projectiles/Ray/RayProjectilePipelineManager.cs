using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: rethink if pipeline is invoked in the correct order respecting Raycaster logic
public class RayProjectilePipelineManager : IDisposable
{
    private ProjectilePhasePipeline _pipeline;
    private ProjectileInitializer _initializer;
    private IProjectile _facade;

    public RayProjectilePipelineManager(ProjectilePhasePipeline pipeline, ProjectileInitializer initializer, IProjectile facade)
    {
        _pipeline = pipeline;
        _initializer = initializer;
        _facade = facade;
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

    private void OnProjectileInitialized(ProjectileSpawnParameters parameters)
    {
        _pipeline.SetState(ProjectilePhases.Created, new ProjectilePipelineParameters(_facade, parameters));
    }

    private void OnProjectileDestroyed(ProjectileSpawnParameters parameters)
    {
        _pipeline.SetState(ProjectilePhases.Destroyed, new ProjectilePipelineParameters(_facade, parameters));
    }
}
