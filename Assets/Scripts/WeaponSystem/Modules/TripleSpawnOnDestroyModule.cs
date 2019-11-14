using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TripleSpawnOnDestroyModule : BaseModule
{
    public override int Priority => 1;

    private float _spreadAngle = 30.0f;
    private IFactory<ProjectileSpawnParameters, Projectile[]> _factory;

    public TripleSpawnOnDestroyModule(IFactory<ProjectileSpawnParameters, Projectile[]> factory)
    {
        _factory = factory;
    }

    public override bool AttachToWeapon(IWeapon weapon)
    {
        if (base.AttachToWeapon(weapon))
        {
            if (_factory == null)
            {
                _factory = weapon.Factory;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool DetachFromWeapon(IWeapon weapon)
    {
        if (base.DetachFromWeapon(weapon))
        {
            if (_factory == weapon.Factory)
            {
                _factory = null;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void DecorateProjectile(Projectile projectile)
    {
        projectile.Pipeline.SubscribeToInit(ProjectilePhases.Destroy, OnProjectileDestroyed);
    }

    public override void RemoveFromProjectile(Projectile projectile)
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
