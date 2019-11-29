using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TripleShot : BaseModule
{
    public override int Priority => 0;
    private Factory _factory = new Factory();

    public override bool AttachToWeapon(IWeapon weapon)
    {
        if (base.AttachToWeapon(weapon))
        {
            _factory.DecoratedFactory = weapon.Factory;
            weapon.Factory = _factory;
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool DetachFromWeapon(IWeapon weapon)
    {
        if (base.DetachFromWeapon(weapon))
        {
            weapon.Factory = _factory.DecoratedFactory;
            return true;
        }
        else
        {
            return false;
        }
    }

    public class Factory : IFactory<ProjectileSpawnParameters, IProjectile[]>
    {
        private List<IProjectile> _objects = new List<IProjectile>();
        private float _spreadAngle = 30.0f;
        public IFactory<ProjectileSpawnParameters, IProjectile[]> DecoratedFactory { get; set; }
        public IProjectile[] Create(ProjectileSpawnParameters param)
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
