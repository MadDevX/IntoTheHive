using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AIAimTowardsRandomPlayerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        InstallAI();
        InstallComponents();
    }

    private void InstallComponents()
    {

    }

    private void InstallAI()
    {
        Container.BindInterfacesAndSelfTo<AIAimTowardsRandomPlayerInput>().AsSingle();
        Container.BindInterfacesAndSelfTo<TargetRandomPlayer>().AsSingle();
    }
}
