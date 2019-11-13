using DarkRift.Client.Unity;
using DarkRift.Server.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

//Make Fields from this class not destroy on load except Network Initializer
public class GlobalNetworkInstaller : MonoInstaller
{
    [SerializeField] private UnityClient _client;

    public override void InstallBindings()
    {
        Container.Bind<UnityClient>().FromInstance(_client).AsSingle();
        Container.BindInterfacesAndSelfTo<ClientInfo>().AsSingle();
        Container.BindInterfacesAndSelfTo<NetworkRelay>().AsSingle();
        Container.BindInterfacesAndSelfTo<HostManager>().AsSingle();        
        Container.BindInterfacesAndSelfTo<NetworkedSceneManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<ChangeSceneMessageSender>().AsSingle();
    }
}
