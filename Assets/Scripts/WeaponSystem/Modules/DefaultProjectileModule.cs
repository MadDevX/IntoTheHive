using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for default projectile behaviours: <para/>
/// <br>Destroy after collision, </br>
/// <br>Destroy after time (not yet implemented)</br>
/// </summary>
public static class DefaultProjectileModule
{
    public static void DecorateProjectile(IProjectile projectile)
    {
        projectile.OnCollisionEnter += DestroyAfterCollisions;
    }

    public static void RemoveFromProjectile(IProjectile projectile)
    {

        projectile.OnCollisionEnter -= DestroyAfterCollisions;
    }

    private static void DestroyAfterCollisions(IProjectile projectile, Collider2D hit, int remainingCollisions)
    {
        if(remainingCollisions < 0)
        {
            projectile.Destroy();
        }
    }
}
