using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModule
{
    /// <summary>
    /// Module sort is based on priority (with 0 being the highest priority, int.maxvalue the lowest priority)
    /// </summary>
    int Priority { get; }

    short Id { get; }
    void DecorateProjectile(IProjectile projectile);
    void RemoveFromProjectile(IProjectile projectile);

    bool AttachToWeapon(IWeapon weapon);
    bool DetachFromWeapon();

    bool IsAttached { get; }
}

public enum Priorities
{
    Essential,
    Overriding,
    Dependant,
    Additive
}
