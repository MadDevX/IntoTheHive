using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

public class PlayerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        InstallPlayer();
    }

    private void InstallPlayer()
    {
        Container.BindInterfacesAndSelfTo<PlayerInput>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerRegistryTracker>().AsSingle();
    }
}
