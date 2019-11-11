using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TripleShot : IModule
{
    public int Priority => 0;
    private IWeapon _weapon;
    private Factory _factory = new Factory();

    public void AttachToWeapon(IWeapon weapon)
    {
        if (_weapon != null)
        {
            Debug.LogError("mod already attached!");
        }
        else
        {
            _weapon = weapon;
            _factory.DecoratedFactory = weapon.Factory;
            weapon.Factory = _factory;
        }
    }

    public void DetachFromWeapon(IWeapon weapon)
    {
        if (_weapon == null)
        {
            Debug.LogError("mod alread detached!");
        }
        else
        {
            weapon.Factory = _factory.DecoratedFactory;
            _weapon = null;
        }
    }

    public void RemoveFromProjectile(Projectile projectile)
    {
    }

    public void DecorateProjectile(Projectile projectile)
    {
    }

    public class Factory : IFactory<ProjectileSpawnParameters, Projectile[]>
    {
        private List<Projectile> _objects = new List<Projectile>();
        private float _spreadAngle = 30.0f;
        public IFactory<ProjectileSpawnParameters, Projectile[]> DecoratedFactory { get; set; }
        public Projectile[] Create(ProjectileSpawnParameters param)
        {
            _objects.Clear();
            var initRotation = param.rotation;
            _objects.AddRange(DecoratedFactory.Create(param));
            param.rotation = initRotation + _spreadAngle;
            _objects.AddRange(DecoratedFactory.Create(param));
            param.rotation = initRotation - _spreadAngle;
            _objects.AddRange(DecoratedFactory.Create(param));
            return _objects.ToArray();
        }
    }
}
