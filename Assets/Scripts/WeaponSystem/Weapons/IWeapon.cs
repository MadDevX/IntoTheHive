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

    void AttachModule(IModule module);
    void DetachModule(IModule module);

    void SetModules(List<IModule> modules);

    event Action<List<IModule>> OnWeaponRefreshed;
    IFactory<ProjectileSpawnParameters, IProjectile[]> Factory { get; set; }
}
