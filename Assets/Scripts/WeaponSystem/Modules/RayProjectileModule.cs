using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RayProjectileModule : BaseModule
{
    public override int Priority => 0;

    public override short Id => 4;

    public override bool IsInheritable => false;

    private IFactory<ProjectileSpawnParameters, ProjectileFacade[]> _factory;

    private IFactory<ProjectileSpawnParameters, IProjectile[]> _weaponFactory;

    [Inject]
    public void Construct([Inject(Id = Identifiers.Ray)] IFactory<ProjectileSpawnParameters, ProjectileFacade[]> factory)
    {
        _factory = factory;
    }

    protected override void OnAttach()
    {
        _weaponFactory = _weapon.Factory;
        _weapon.Factory = _factory;
    }

    protected override void OnDetach()
    {
        _weapon.Factory = _weaponFactory;
        _weaponFactory = null;
    }

    public override IModule Clone()
    {
        var clone = new RayProjectileModule();
        clone.Construct(_factory);
        return clone;
    }
}
