using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Weapon : IWeapon
{
    public IFactory<ProjectileSpawnParameters, IProjectile[]> Factory { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public event Action<List<IModule>> OnWeaponRefreshed;

    public void AttachModule(IModule module)
    {
        throw new System.NotImplementedException();
    }

    public void DetachModule(IModule module)
    {
        throw new System.NotImplementedException();
    }

    public void ReleaseTrigger()
    {
        throw new System.NotImplementedException();
    }

    public void Reload()
    {
        throw new System.NotImplementedException();
    }

    public bool Shoot(Vector2 position, float rotation, Vector2 offset)
    {
        throw new System.NotImplementedException();
    }
}
