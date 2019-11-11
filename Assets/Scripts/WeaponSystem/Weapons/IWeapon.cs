using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IWeapon
{
    bool Shoot(Vector2 position, float rotation, Vector2 offset);
    void ReleaseTrigger();
    void Reload();

    void AttachModule(IModule module);
    void DetachModule(IModule module);

    event Action<List<IModule>> OnWeaponRefreshed;
    IFactory<ProjectileSpawnParameters, Projectile[]> Factory { get; set; }
}
