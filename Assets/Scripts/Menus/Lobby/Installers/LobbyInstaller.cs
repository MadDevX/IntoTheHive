using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LobbyInstaller : MonoInstaller
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _readyButton;
    [SerializeField] private Button _leaveLobbyButton;
    [SerializeField] private SceneInitializedAnnouncer _sceneInitializedAnnouncer;

    public override void InstallBindings()
    {
        InstallButtons();
        InstallMessageHandling();
        InstallInitializationHandling();
        InstallLobbyState();
        
        Container.BindInterfacesAndSelfTo<LobbyMenuManager>().AsSingle();
    }

    /// <summary>
    /// Installs LobbyState used to track players in lobby and their ready status
    /// </summary>
    private void InstallLobbyState()
    {
        Container.BindInterfacesAndSelfTo<LobbyState>().AsSingle();
        Container.BindInterfacesAndSelfTo<LobbyStateManager>().AsCached();
    }

    private void InstallButtons()
    {
        Container.BindInstance(_startGameButton).WithId(Identifiers.LobbyStartGameButton);
        Container.BindInstance(_readyButton).WithId(Identifiers.LobbyReadyButton);
        Container.BindInstance(_leaveLobbyButton).WithId(Identifiers.LobbyLeaveButton);
    }

    private void InstallMessageHandling()
    {
        Container.BindInterfacesAndSelfTo<LobbyMessageSender>().AsSingle();        
        Container.BindInterfacesAndSelfTo<LobbyHostMessageReceiver>().AsSingle();
        Container.BindInterfacesAndSelfTo<LobbyClientMessageReceiver>().AsSingle();
    }

    /// <summary>
    /// Installs SceneInitializedAnnouncer which is executed last from all the MonoBehaviours and handlers which respond to his start
    /// </summary>
    private void InstallInitializationHandling()
    {
        Container.BindInstance(_sceneInitializedAnnouncer);
        Container.BindInterfacesAndSelfTo<LobbyInitializedHandler>().AsSingle();
    }
}

