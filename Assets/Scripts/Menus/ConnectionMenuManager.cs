using DarkRift.Client.Unity;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class ConnectionMenuManager : IInitializable, IDisposable
{
    private Button _serverButton;
    private Button _joinButton;
    private Button _backButton;

    private ClientInfo _clientInfo;
    private ServerManager _serverManager;
    private NetworkedSceneManager _networkedSceneManager;
    private ChangeSceneMessageSender _sceneMessageSender;
    private NetworkedClientInitializer _initializer;

    public ConnectionMenuManager(
        [Inject(Id = Identifiers.ConnetionMenuCreateServerButton)] Button serverButton,
        [Inject(Id = Identifiers.ConnetionMenuJoinServerButton)] Button joinButton,
        [Inject(Id = Identifiers.ConnetionMenuBackButton)] Button backButton,
        NetworkedClientInitializer connectionInitializer,
        NetworkedSceneManager networkedSceneManager,
        ChangeSceneMessageSender sceneMessageSender,
        ServerManager serverManager,
        ClientInfo clientInfo)
    {
        _serverButton = serverButton;
        _joinButton = joinButton;
        _backButton = backButton;

        _clientInfo = clientInfo;
        _serverManager = serverManager;
        _sceneMessageSender = sceneMessageSender;
        _networkedSceneManager = networkedSceneManager;
        _initializer = connectionInitializer;
    }

    public void Initialize()
    {
        _clientInfo.Status = ClientStatus.None;
        _serverButton.onClick.AddListener(ServerButtonClicked);
        _joinButton.onClick.AddListener(JoinButtonClicked);
        _backButton.onClick.AddListener(BackButtonClicker);
        
    }

    public void Dispose()
    {
        _serverButton.onClick.RemoveListener(ServerButtonClicked);
        _joinButton.onClick.RemoveListener(JoinButtonClicked);
        _backButton.onClick.RemoveListener(BackButtonClicker);
    }

    public void ServerButtonClicked()
    {
        _serverManager.CreateServer();
        _serverManager.JoinAsHost();
        // TODO MG: change this maybe to via a message when connected to a server
        _clientInfo.Status = ClientStatus.Host;
        _sceneMessageSender.SendSceneChanged("Lobby", true); // Send message to load Lobby Client
        SceneManager.LoadScene("Lobby"); // Load LobbyHost
    }

    public void JoinButtonClicked()
    {
        //_initializer.JoinServer();
        // TODO MG: change this maybe to via a message when connected to a server
        _clientInfo.Status = ClientStatus.Client;
        SceneManager.LoadScene("Lobby");
    }

    public void BackButtonClicker()
    {
        SceneManager.LoadScene("MainMenu");
    }


}