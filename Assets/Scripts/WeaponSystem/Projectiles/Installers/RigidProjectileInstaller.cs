using Relays;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RigidProjectileInstaller : MonoInstaller
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private ProjectileRelay _relay;

    public override void InstallBindings()
    {
        InstallComponents();
        InstallRelay();
        InstallProjectile();
    }


    private void InstallComponents()
    {
        Container.BindInstance(_collider).AsSingle();
        Container.BindInstance(_rb).AsSingle();
        Container.BindInstance(_trailRenderer).AsSingle();
    }

    private void InstallRelay()
    {
        Container.Bind<IRelay>().FromInstance(_relay).AsSingle();
    }

    private void InstallProjectile()
    {
        Container.BindInterfacesAndSelfTo<ProjectileCollisionHandler>().AsSingle();
    }
}
