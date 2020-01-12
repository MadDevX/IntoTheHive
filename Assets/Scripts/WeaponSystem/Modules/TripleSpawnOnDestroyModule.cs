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

    private IFactory<ProjectileSpawnParameters, IProjectile[]> _factory = null;
    private Settings _settings;
    
    [Inject]
    public void Construct(Settings settings)
    {
        _settings = settings;
    }

    protected override void OnAttach()
    {
        _factory = _weapon.BaseFactory;
    }

    protected override void OnDetach()
    {
        _factory = null;
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
        var spawnParam = new ProjectileSpawnParameters(spawnPos, baseRotation, velocity.magnitude, _settings.ttl, param.inheritableModules, param.inheritableModules, param.projectile.IsDummy);
        _factory.Create(spawnParam);
        spawnParam.rotation = baseRotation + _settings.spreadAngle;
        _factory.Create(spawnParam);
        spawnParam.rotation = baseRotation - _settings.spreadAngle;
        _factory.Create(spawnParam);
    }

    public override IModule Clone()
    {
        var clone = new TripleSpawnOnDestroyModule();
        clone.Construct(_settings);
        return clone;
    }

    [System.Serializable]
    public class Settings
    {
        public float ttl;
        public float spreadAngle;
    }
}
