using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectileInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ProjectilePhasePipeline>().AsSingle();
    }
}
