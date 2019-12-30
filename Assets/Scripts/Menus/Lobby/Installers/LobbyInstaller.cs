using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LobbyInstaller : MonoInstaller
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _readyButton;
    [SerializeField] private Button _leaveLobbyButton;
    [SerializeField] private Text _startTextReady;
    [SerializeField] private Text _startTextNotReady;
    [SerializeField] private PlayerEntryFacade _playerEntryPrefab;
    [SerializeField] private Transform _playerEntryPanel;
    [SerializeField] private Text _ipText;
    [SerializeField] private Text _portText;

    [SerializeField] private SceneInitializedAnnouncer _sceneInitializedAnnouncer;

    public override void InstallBindings()
    {
        InstallButtons();
        InstallMessageHandling();
        InstallInitializationHandling();
        InstallLobbyState();
        InstallPlayerList();
        Container.BindInterfacesAndSelfTo<LobbyMenuManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<LobbyInitializer>().AsSingle();
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
        Container.BindInstance(_startTextReady).WithId(Identifiers.StartTextReady);
        Container.BindInstance(_startTextNotReady).WithId(Identifiers.StartTextNotReady);
        Container.BindInstance(_ipText).WithId(Identifiers.IP);
        Container.BindInstance(_portText).WithId(Identifiers.Port);
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
        Container.BindInterfacesAndSelfTo<SceneInitializedBaseHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<ServerInfoTracker>().AsSingle();
    }

    private void InstallPlayerList()
    {
        Container.BindInterfacesAndSelfTo<PlayerEntryManager>().AsSingle();
        BindMonoPrefabPool<PlayerEntryFacade, PlayerEntrySpawnParameters, PlayerEntryFacade.Factory, PlayerEntryPool>
            (Identifiers.PlayerEntryPool, 4, _playerEntryPrefab, _playerEntryPanel);
    }

    private void BindMonoPrefabPool<T, TArgs, TFactory, TPool>(Identifiers id, int size, T prefab, Transform parentTransform, BindingCondition cond = null)
   where T : MonoBehaviour, IPoolable<TArgs, IMemoryPool>
   where TFactory : PlaceholderFactory<TArgs, T>
   where TPool : MonoPoolableMemoryPool<TArgs, IMemoryPool, T>
    {
        var bind =
        Container.BindFactory<TArgs, T, TFactory>().
            WithId(id).
            FromPoolableMemoryPool<TArgs, T, TPool>
            (x => x.WithInitialSize(size).
            ExpandByDoubling().
            FromComponentInNewPrefab(prefab).
            UnderTransform(parentTransform));

        if (cond != null)
        {
            bind.When(cond);
        }
    }

    public class PlayerEntryPool : MonoPoolableMemoryPool<PlayerEntrySpawnParameters, IMemoryPool, PlayerEntryFacade>
    {
    }
}

