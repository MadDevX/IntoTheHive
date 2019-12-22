using System;
using DarkRift.Client.Unity;
using UnityEngine;
using Zenject;

public class ProjectNetworkInstaller : MonoInstaller
{
    [SerializeField] private UnityClient _client;

    public override void InstallBindings()
    {
        InstallConnectionManagment();
        InstallScenesNetworkedManagment();
        InstallMessageManagment();
        InstallPlayerManagment();
        InstallLevelManagment();                    
    }

    private void InstallConnectionManagment()
    {
        // Connection
        Container.Bind<UnityClient>().FromInstance(_client).AsSingle();
        Container.BindInterfacesAndSelfTo<ClientInfo>().AsSingle();
        Container.BindInterfacesAndSelfTo<NetworkRelay>().AsSingle();
        Container.BindInterfacesAndSelfTo<DisconnectionManager>().AsSingle();
    }

    private void InstallScenesNetworkedManagment()
    {
        // Scenes
        Container.BindInterfacesAndSelfTo<HostSceneManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<SceneMessageReceiver>().AsSingle();
        Container.BindInterfacesAndSelfTo<SceneMessageSender>().AsSingle();
        Container.BindInterfacesAndSelfTo<SynchronizedSceneManager>().AsSingle();
    }

    private void InstallMessageManagment()
    {
        // Generic Messages with response
        Container.BindInterfacesAndSelfTo<GenericMessageWithResponseHost>().AsSingle();
        Container.BindInterfacesAndSelfTo<GenericMessageWithResponseClient>().AsSingle();
    }

    private void InstallPlayerManagment()
    {
        // Players and characters
        Container.BindInterfacesAndSelfTo<GlobalHostPlayerManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<NetworkedCharacterSpawner>().AsSingle();
    }

    private void InstallLevelManagment()
    {
        // Level generation
        Container.BindInterfacesAndSelfTo<LevelGraphMessageSender>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelGraphState>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelGraphGenerator>().AsSingle();
    }
}
