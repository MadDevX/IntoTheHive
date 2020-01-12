using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IWeapon
{
    event Action<ProjectileSpawnParameters> OnShoot;
    bool Shoot(Vector2 position, float rotation);
    void ReleaseTrigger();
    void Reload();

    /// <summary>
    /// Attaches <paramref name="module"/> to weapon and refreshes weapon state.
    /// </summary>
    /// <param name="module"></param>
    void AttachModule(IModule module);

    /// <summary>
    /// Detaches <paramref name="module"/> from weapon and refreshes weapon state.
    /// </summary>
    /// <param name="module"></param>
    void DetachModule(IModule module);

    /// <summary>
    /// Detaches any previous modules and attaches every module contained in the <paramref name="modules"/> list.
    /// Refreshes weapon after attaching entire list.
    /// </summary>
    /// <param name="modules"></param>
    void SetModules(List<IModule> modules);

    event Action<List<IModule>> OnWeaponRefreshed;

    /// <summary>
    /// Potentially decorated factory used by weapon to spawn projectiles.
    /// </summary>
    IFactory<ProjectileSpawnParameters, IProjectile[]> Factory { get; set; }

    /// <summary>
    /// Modules overriding base factory should have Priority set to 0. 
    /// BaseFactory should represent direct projectile prefab factory.
    /// </summary>
    IFactory<ProjectileSpawnParameters, IProjectile[]> BaseFactory { get; set; }
}
