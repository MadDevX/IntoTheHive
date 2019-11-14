﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModule
{
    /// <summary>
    /// Module sort is based on priority (with 0 being the highest priority, int.maxvalue the lowest priority)
    /// </summary>
    int Priority { get; }
    void DecorateProjectile(Projectile projectile);
    void RemoveFromProjectile(Projectile projectile);

    bool AttachToWeapon(IWeapon weapon);
    bool DetachFromWeapon(IWeapon weapon);
}

public enum Priorities
{
    Essential,
    Overriding,
    Dependant,
    Additive
}
