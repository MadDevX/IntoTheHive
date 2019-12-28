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
        // TODO MG - decide how to reference Character Spawner from ConnectionMenu
        //Container.BindInterfacesAndSelfTo<NetworkedCharacterSpawner>().AsSingle();
        Container.BindInterfacesAndSelfTo<PickupSpawnMessageHandler>().AsSingle();
    }
}
