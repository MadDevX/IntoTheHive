using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectileInstaller : MonoInstaller
{
    [SerializeField] private ProjectileFacade _facade;

    public override void InstallBindings()
    {
        InstallProjectile();
    }

    private void InstallProjectile()
    {
        Container.Bind(typeof(IProjectile), typeof(ProjectileFacade)).FromInstance(_facade).AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectilePhasePipeline>().AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectileModules>().AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectileInitializer>().AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectileDestroyAfterCollision>().AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectileDamage>().AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectileHit>().AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectileDummy>().AsSingle();
    }
}
