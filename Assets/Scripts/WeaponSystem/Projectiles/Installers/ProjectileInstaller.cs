using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Music;
using UnityEngine;
using Zenject;

public class ProjectileInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        InstallProjectile();
        InstallComponents();
        InstallSFX();
    }

    private void InstallSFX()
    {
        Container.BindInterfacesAndSelfTo<SFXPlayer>().AsSingle();
    }

    private void InstallComponents()
    {
        Container.Bind<Transform>().FromInstance(transform).AsSingle();
        Container.Bind<GameObject>().FromInstance(gameObject).AsSingle();
    }

    private void InstallProjectile()
    {
        Container.Bind(typeof(IProjectile), typeof(ProjectileFacade)).To<ProjectileFacade>().AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectilePhasePipeline>().AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectileModules>().AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectileInitializer>().AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectileDestroyAfterCollision>().AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectileDamage>().AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectileHit>().AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectileDummy>().AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectileFloatingText>().AsSingle();
        Container.BindInterfacesAndSelfTo<ProjectileVFX>().AsSingle();
    }
}
