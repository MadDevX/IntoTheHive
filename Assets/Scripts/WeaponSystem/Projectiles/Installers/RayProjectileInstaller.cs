using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RayProjectileInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<RayProjectileLocation>().AsSingle();
        Container.BindInterfacesAndSelfTo<RayProjectileCollisionHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<RayProjectilePipelineManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<RayProjectileTimers>().AsSingle();
        Container.BindInterfacesAndSelfTo<RayProjectileVFX>().AsSingle();
        Container.BindInterfacesAndSelfTo<RayProjectileRaycaster>().AsSingle();
    }
}
