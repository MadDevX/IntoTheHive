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
            if (_factory == null)
            {
                _factory = weapon.Factory;
            }
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
            if(_factory == weapon.Factory)
            {
                _factory = null;
            }
        }
    }

    public void RemoveFromProjectile(Projectile projectile)
    {
        projectile.Pipeline.UnsubscribeFromInit(ProjectilePhases.Destroy, OnProjectileDestroyed);
    }

    private void OnProjectileDestroyed(ProjectilePipelineParameters param)
    {
        var velocity = param.rb.velocity;
        var baseRotation = velocity.Rotation();
        var spawnPos = param.projectile.Position + Vector2.ClampMagnitude(velocity, Constants.COLLISION_CORRECTION_EPS);
        var spawnParam = new ProjectileSpawnParameters(spawnPos, baseRotation, velocity.magnitude, 3.0f, null);
        _factory.Create(spawnParam);
        spawnParam.rotation = baseRotation + _spreadAngle;
        _factory.Create(spawnParam);
        spawnParam.rotation = baseRotation - _spreadAngle;
        _factory.Create(spawnParam);
    }
}
