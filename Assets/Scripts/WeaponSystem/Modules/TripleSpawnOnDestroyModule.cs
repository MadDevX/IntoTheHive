using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TripleSpawnOnDestroyModule : IModule
{
    public int Priority => 1;
    private float _spreadAngle = 30.0f;
    private IWeapon _weapon;
    private IFactory<ProjectileSpawnParameters, Projectile[]> _factory;

    public TripleSpawnOnDestroyModule(IFactory<ProjectileSpawnParameters, Projectile[]> factory)
    {
        _factory = factory;
    }

    public void AttachToWeapon(IWeapon weapon)
    {
        if (_weapon != null)
        {
            Debug.LogError("mod already attached!");
        }
        else
        {
            _weapon = weapon;
            _factory = weapon.Factory;
        }
    }

    public void DecorateProjectile(Projectile projectile)
    {
        projectile.Pipeline.SubscribeToInit(ProjectilePhases.Destroy, OnProjectileDestroyed);
    }

    public void DetachFromWeapon(IWeapon weapon)
    {
        if(_weapon == null)
        {
            Debug.LogError("mod already detached!");
        }
        else
        {
            _weapon = null;
        }
    }

    public void RemoveFromProjectile(Projectile projectile)
    {
        projectile.Pipeline.UnsubscribeFromInit(ProjectilePhases.Destroy, OnProjectileDestroyed);
    }

    private void OnProjectileDestroyed(ProjectilePipelineParameters param)
    {
        var baseRotation = param.physics.Velocity.Rotation();
        var spawnParam = new ProjectileSpawnParameters(param.projectile.Position, baseRotation, param.physics.Velocity.magnitude, 3.0f, null);
        _factory.Create(spawnParam); //TODO: memory pool spawn overlap? check if it's a bug aight
        _factory.Create(spawnParam);
        spawnParam.rotation = baseRotation + _spreadAngle;
        _factory.Create(spawnParam);
        spawnParam.rotation = baseRotation - _spreadAngle;
        _factory.Create(spawnParam);
    }
}
