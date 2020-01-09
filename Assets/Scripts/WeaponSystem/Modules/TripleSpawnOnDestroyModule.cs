using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TripleSpawnOnDestroyModule : BaseModule
{
    public override int Priority => 2;

    public override short Id => 1;

    public override bool IsInheritable => false;

    private float _spreadAngle = 10.0f;
    private IFactory<ProjectileSpawnParameters, IProjectile[]> _factory;

    public TripleSpawnOnDestroyModule()
    {
        _factory = null;
    }

    public TripleSpawnOnDestroyModule(IFactory<ProjectileSpawnParameters, IProjectile[]> factory)
    {
        _factory = factory;
    }

    protected override void OnAttach()
    {
        if(_factory == null)
        {
            _factory = _weapon.Factory;
        }
    }

    protected override void OnDetach()
    {
        if(_factory == _weapon.Factory)
        {
            _factory = null;
        }
    }

    public override void DecorateProjectile(IProjectile projectile)
    {
        projectile.Pipeline.SubscribeToInit(ProjectilePhases.Destroyed, OnProjectileDestroyed);
    }

    public override void RemoveFromProjectile(IProjectile projectile)
    {
        projectile.Pipeline.UnsubscribeFromInit(ProjectilePhases.Destroyed, OnProjectileDestroyed);
    }

    private void OnProjectileDestroyed(ProjectilePipelineParameters param)
    {
        var velocity = param.projectile.Velocity;
        var baseRotation = velocity.Rotation();
        var spawnPos = param.projectile.Position + Vector2.ClampMagnitude(velocity, Constants.COLLISION_CORRECTION_EPS);
        //TODO: remove magic number representing projectile TimeToLive
        var spawnParam = new ProjectileSpawnParameters(spawnPos, baseRotation, velocity.magnitude, 3.0f, param.inheritableModules, param.inheritableModules, param.projectile.IsDummy);
        _factory.Create(spawnParam);
        spawnParam.rotation = baseRotation + _spreadAngle;
        _factory.Create(spawnParam);
        spawnParam.rotation = baseRotation - _spreadAngle;
        _factory.Create(spawnParam);
    }

    public override IModule Clone()
    {
        if (_weapon == null)
        {
            if (_factory == null)
            {
                return new TripleSpawnOnDestroyModule();
            }
            else
            {
                return new TripleSpawnOnDestroyModule(_factory);
            }
        }
        else
        {
            throw new InvalidOperationException("Tried to clone attached module. This must not be done outside ModuleDictionary class");
        }
    }
}
