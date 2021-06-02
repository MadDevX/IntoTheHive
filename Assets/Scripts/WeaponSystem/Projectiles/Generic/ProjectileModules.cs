using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectileModules : IDisposable
{
    private IProjectile _facade;
    private ProjectileInitializer _initializer;

    private List<IModule> _currentModules = new List<IModule>();

    public ProjectileModules(IProjectile facade, ProjectileInitializer initializer)
    {
        _facade = facade;
        _initializer = initializer;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _initializer.OnProjectileCreated += ProjectileCreated;
        _initializer.OnProjectileDespawned += ProjectileDespawned;
    }

    public void Dispose()
    {
        _initializer.OnProjectileCreated -= ProjectileCreated;
        _initializer.OnProjectileDespawned -= ProjectileDespawned;
    }

    private void ProjectileCreated(ProjectileSpawnParameters parameters)
    {
        SetModules(parameters.modules);
        InitModules();
    }

    private void ProjectileDespawned()
    {
        DisposeModules();
        //clear modules maybe
    }

    public void SetModules(List<IModule> modules)
    {
        _currentModules.Clear();
        if (modules != null)
        {
            _currentModules.AddRange(modules);
        }
    }


    private void InitModules()
    {
        for (int i = 0; i < _currentModules.Count; i++)
        {
            _currentModules[i].DecorateProjectile(_facade);
        }
    }

    private void DisposeModules()
    {
        for (int i = _currentModules.Count - 1; i >= 0; i--)
        {
            _currentModules[i].RemoveFromProjectile(_facade);
        }
    }
}
