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
        Container.BindInterfacesAndSelfTo<DisconnectionManager>().AsSingle();

        Container.BindInterfacesAndSelfTo<SceneMessageReceiver>().AsSingle();
        Container.BindInterfacesAndSelfTo<SceneMessageSender>().AsSingle();
        Container.BindInterfacesAndSelfTo<SceneMessageWithResponse>().AsSingle();

        Container.BindInterfacesAndSelfTo<GlobalHostPlayerManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<NetworkedCharacterSpawner>().AsSingle();

        //TODO MG : make some mechanic that allows for interscene communication without. Maybe a signal bus?
        Container.BindInterfacesAndSelfTo<LevelGraphMessageSender>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelGraphState>().AsSingle();
        Container.BindInterfacesAndSelfTo<BasicLevelGraphGenerator>().AsSingle();
    }
}
