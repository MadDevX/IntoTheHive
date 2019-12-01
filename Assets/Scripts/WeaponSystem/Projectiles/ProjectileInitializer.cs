using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInitializer
{
    /// <summary>
    /// Invoked immediately after create projectile call
    /// </summary>
    public event Action<ProjectileSpawnParameters> OnProjectileCreated;
    /// <summary>
    /// Invoked after all projectile creation handlers are invoked (used for resetting bullet state after module decoration)
    /// </summary>
    public event Action OnProjectileInitialized;

    public event Action OnProjectileDespawned;

    public void CreateProjectile(ProjectileSpawnParameters parameters)
    {
        OnProjectileCreated?.Invoke(parameters);
        OnProjectileInitialized?.Invoke();
    }

    public void DespawnProjectile()
    {
        OnProjectileDespawned?.Invoke();
    }
}
