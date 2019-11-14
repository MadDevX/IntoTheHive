using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineTravelModule : BaseModule
{
    public override int Priority => 1;

    private static float _offset = 2.0f / 3.0f;
    public override void DecorateProjectile(Projectile projectile)
    {
        projectile.OnFixedUpdateEvt += OnFixedUpdate;
        //projectile.Velocity = projectile.Velocity.Rotate(-45.0f);
    }

    public override void RemoveFromProjectile(Projectile projectile)
    {
        projectile.OnFixedUpdateEvt -= OnFixedUpdate;
    }

    private void OnFixedUpdate(Projectile projectile, float deltaTime)
    {
        projectile.Velocity = projectile.Velocity.Rotate(-Mathf.Sin(projectile.FixedTravelTime * 50.0f + Mathf.PI * _offset) * 45.0f);
    }
}
