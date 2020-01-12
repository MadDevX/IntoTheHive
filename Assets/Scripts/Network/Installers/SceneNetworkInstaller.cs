using DarkRift.Client.Unity;
using Networking.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SceneNetworkInstaller : MonoInstaller
{
    public override void InstallBindings()
    {        
        Container.BindInterfacesAndSelfTo<PickupSpawnMessageHandler>().AsSingle();
    }
}
