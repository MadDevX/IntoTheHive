using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseModule : IModule
{
    protected IWeapon _weapon;
    public abstract int Priority { get; }

    public virtual bool AttachToWeapon(IWeapon weapon)
    {
        if (_weapon != null)
        {
            Debug.LogError("mod already attached!");
            return false;
        }
        else
        {
            _weapon = weapon;
            return true;
        }
    }
    public virtual bool DetachFromWeapon(IWeapon weapon)
    {
        if (_weapon == null)
        {
            Debug.LogError("mod already detached!");
            return false;
        }
        else
        {
            _weapon = null;
            return true;
        }
    }
    public virtual void DecorateProjectile(IProjectile projectile) { }
    public virtual void RemoveFromProjectile(IProjectile projectile) { }
}
