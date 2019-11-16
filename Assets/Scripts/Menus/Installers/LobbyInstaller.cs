using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LobbyInstaller : MonoInstaller
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _readyButton;
    [SerializeField] private Button _leaveLobbyButton;

    public override void InstallBindings()
    {
        Container.BindInstance(_startGameButton).WithId(Identifiers.LobbyStartGameButton);
        Container.BindInstance(_readyButton).WithId(Identifiers.LobbyReadyButton);
        Container.BindInstance(_leaveLobbyButton).WithId(Identifiers.LobbyLeaveButton);

        Container.BindInterfacesAndSelfTo<LobbyState>().AsSingle();
        Container.BindInterfacesAndSelfTo<LobbyMenuManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<LobbyMessageSender>().AsSingle();
        Container.BindInterfacesAndSelfTo<HostLobbyManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<ClientLobbyManager>().AsSingle();
        
    }
}

