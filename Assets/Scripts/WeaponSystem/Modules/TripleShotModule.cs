using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TripleShot : BaseModule
{
    public override int Priority => 0;

    public override short Id => 0;

    public override bool IsInheritable => false;

    private Factory _factory = new Factory();

    protected override void OnAttach()
    {
        _factory.DecoratedFactory = _weapon.Factory;
        _weapon.Factory = _factory;
    }

    protected override void OnDetach()
    {
        _weapon.Factory = _factory.DecoratedFactory;
    }

    public override IModule Clone()
    {
        return new TripleShot();
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
