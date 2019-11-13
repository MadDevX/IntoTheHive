using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DefaultProjectileModule
{
    public static void DecorateProjectile(Projectile projectile)
    {
        projectile.OnCollisionEnter += DestroyAfterCollisions;
    }

    public static void RemoveFromProjectile(Projectile projectile)
    {

        projectile.OnCollisionEnter -= DestroyAfterCollisions;
    }

    private static void DestroyAfterCollisions(Projectile projectile, Collider2D hit, int remainingCollisions)
    {
        if(remainingCollisions < 0)
        {
            projectile.Dispose();
        }
    }
}
