using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineTravelModule : BaseModule
{
    public override int Priority => 1;

    public override short Id => 2;

    public override bool IsInheritable => false;

    private static float _offset = 2.0f / 3.0f;
    public override void DecorateProjectile(IProjectile projectile)
    {
        projectile.OnFixedUpdateEvt += OnFixedUpdate;
        //projectile.Velocity = projectile.Velocity.Rotate(-45.0f);
    }

    public override void RemoveFromProjectile(IProjectile projectile)
    {
        projectile.OnFixedUpdateEvt -= OnFixedUpdate;
    }

    private void OnFixedUpdate(IProjectile projectile, float deltaTime)
    {
        projectile.Velocity = projectile.Velocity.Rotate(-Mathf.Sin(projectile.FixedTravelTime * 50.0f + Mathf.PI * _offset) * 45.0f);
    }

    public override IModule Clone()
    {
        return new SineTravelModule();
    }
}
