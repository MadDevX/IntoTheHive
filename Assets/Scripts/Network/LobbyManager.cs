using DarkRift.Client.Unity;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class LobbyManager: IInitializable, IDisposable
{
    private Button _serverButton;
    private Button _joinButton;

    private ServerManager _serverManager;
    private NetworkedSceneManager _networkedSceneManager;
    private NetworkedClientInitializer _initializer;

    public LobbyManager(
        [Inject(Id = Identifiers.CreateServerButton)] Button serverButton,
        [Inject(Id = Identifiers.JoinServerButton)] Button joinButton,
        ServerManager serverManager,
        NetworkedSceneManager networkedSceneManager,
        NetworkedClientInitializer connectionInitializer
        )
    {
        _serverButton = serverButton;
        _joinButton = joinButton;
        _serverManager = serverManager;
        _networkedSceneManager = networkedSceneManager;
        _initializer = connectionInitializer;
    }

    public void Initialize()
    {
        _serverButton.onClick.AddListener(ServerButtonClicked);
        _joinButton.onClick.AddListener(JoinButtonClicked);        
    }

    public void Dispose()
    {
        _serverButton.onClick.RemoveListener(ServerButtonClicked);
        _joinButton.onClick.RemoveListener(JoinButtonClicked);
    }

    public void ServerButtonClicked()
    {
        
        _serverManager.CreateServer();     
        _initializer.JoinServer();
        SceneManager.LoadScene(1);
        // TODO MG make this JoinServer independent of the input fields
    }

    public void JoinButtonClicked()
    {
        _initializer.JoinServer();
        SceneManager.LoadScene(1);
    }

}