using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TripleShot : BaseModule
{
    public override int Priority => 1;

    public override short Id => 0;

    public override bool IsInheritable => false;

    private Settings _settings;
    private Factory _factory;

    [Inject]
    public void Construct(Settings settings)
    {
        _settings = settings;
        _factory = new Factory(_settings);
    }

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
        var clone = new TripleShot();
        clone.Construct(_settings);
        return clone;
    }

    public class Factory : IFactory<ProjectileSpawnParameters, IProjectile[]>
    {
        private List<IProjectile> _objects = new List<IProjectile>();
        private Settings _settings;

        public Factory(Settings settings)
        {
            _settings = settings;
        }

        public IFactory<ProjectileSpawnParameters, IProjectile[]> DecoratedFactory { get; set; }
        public IProjectile[] Create(ProjectileSpawnParameters param)
        {
            _objects.Clear();
            var initRotation = param.rotation;
            _objects.AddRange(DecoratedFactory.Create(param));
            param.rotation = initRotation + _settings.spreadAngle;
            _objects.AddRange(DecoratedFactory.Create(param));
            param.rotation = initRotation - _settings.spreadAngle;
            _objects.AddRange(DecoratedFactory.Create(param));
            return _objects.ToArray();
        }
    }

    [System.Serializable]
    public class Settings
    {
        public float spreadAngle;
    }
}
