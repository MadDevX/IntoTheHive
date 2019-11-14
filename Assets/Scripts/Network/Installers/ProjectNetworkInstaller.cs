using DarkRift.Client.Unity;
using UnityEngine;
using Zenject;

public class ProjectNetworkInstaller : MonoInstaller
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
