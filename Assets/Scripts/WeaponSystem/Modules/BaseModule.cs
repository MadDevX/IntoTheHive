using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseModule : IModule
{
    protected IWeapon _weapon;
    public abstract int Priority { get; }

    public abstract short Id { get; }

    public bool IsAttached => _weapon != null;

    public bool AttachToWeapon(IWeapon weapon)
    {
        if (_weapon != null)
        {
            Debug.LogWarning("mod already attached!");
            return false;
        }
        else
        {
            _weapon = weapon;
            OnAttach();
            return true;
        }
    }
    public bool DetachFromWeapon()
    {
        if (_weapon == null)
        {
            Debug.LogWarning("mod already detached!");
            return false;
        }
        else
        {
            OnDetach();
            _weapon = null;
            return true;
        }
    }
    public virtual void DecorateProjectile(IProjectile projectile) { }
    public virtual void RemoveFromProjectile(IProjectile projectile) { }

    protected virtual void OnDetach() { }
    protected virtual void OnAttach() { }

    public abstract IModule Clone();
}
