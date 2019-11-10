using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AIInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        //TODO: install AI character controller
        Container.BindInterfacesAndSelfTo<AITargetScanner>().AsSingle();
    }
}
