using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInitializer
{
    /// <summary>
    /// Invoked immediately after create projectile call, defines projectile behaviour
    /// </summary>
    public event Action<ProjectileSpawnParameters> OnProjectileCreated;
    
    /// <summary>
    /// Invoked after all projectile behaviour definitions are invoked, resets projectile state according to current behaviour definitions
    /// </summary>
    public event Action OnProjectileDefined;

    /// <summary>
    /// Invoked after resetting projectile state, last step of bullet creation
    /// </summary>
    public event Action OnProjectileInitialized;

    /// <summary>
    /// Invoked on projectile destruction, used for last hit behaviour, not disposing logic
    /// </summary>
    public event Action OnProjectileDestroyed;

    /// <summary>
    /// Invoked after despawning projectile, used to cleanup projectile state
    /// </summary>
    public event Action OnProjectileDespawned;

    public void CreateProjectile(ProjectileSpawnParameters parameters)
    {
        OnProjectileCreated?.Invoke(parameters);
        OnProjectileDefined?.Invoke();
        OnProjectileInitialized?.Invoke();
    }

    public void DespawnProjectile()
    {
        OnProjectileDestroyed?.Invoke();
        OnProjectileDespawned?.Invoke();
    }
}
