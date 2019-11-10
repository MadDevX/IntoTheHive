using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModule
{
    /// <summary>
    /// Module sort is based on priority (with 0 being the highest priority, int.maxvalue the lowest priority)
    /// </summary>
    int Priority { get; }
    void DecorateProjectile(Projectile projectile);

    void Initialize();
}
