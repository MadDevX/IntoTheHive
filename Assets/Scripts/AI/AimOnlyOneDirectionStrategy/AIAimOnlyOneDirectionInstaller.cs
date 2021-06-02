using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AIAimOnlyOneDirectionInstaller : MonoInstaller
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
        Container.BindInterfacesAndSelfTo<AIAimOnlyOneDirectionInput>().AsSingle();
        Container.BindInterfacesAndSelfTo<AIShootOnTimer>().AsSingle();
        Container.BindInterfacesAndSelfTo<TargetNothing>().AsSingle();
    }
}
