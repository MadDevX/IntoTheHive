using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Music;
using UnityEngine;
using Zenject;

public class RayProjectileInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        InstallProjectile();
        InstallSFX();
    }

    private void InstallSFX()
    {
        Container.BindInterfacesAndSelfTo<RaySoundProvider>().AsSingle();
    }

    private void InstallProjectile()
    {
        Container.BindInterfacesAndSelfTo<RayProjectileLocation>().AsSingle();
        Container.BindInterfacesAndSelfTo<RayProjectileCollisionHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<RayProjectilePipelineManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<RayProjectileTimers>().AsSingle();
        Container.BindInterfacesAndSelfTo<RayProjectileVFX>().AsSingle();
        Container.BindInterfacesAndSelfTo<RayProjectileRaycaster>().AsSingle();

    }
}
