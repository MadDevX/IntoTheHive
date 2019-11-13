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

    private ServerManager _serverManager;
    private NetworkedSceneManager _networkedSceneManager;
    private NetworkedClientInitializer _initializer;

    public ConnectionMenuManager (
        [Inject(Id = Identifiers.ConnetionMenuCreateServerButton)] Button serverButton,
        [Inject(Id = Identifiers.ConnetionMenuJoinServerButton)] Button joinButton,
        [Inject(Id = Identifiers.ConnetionMenuBackButton)] Button backButton,
        ServerManager serverManager,
        NetworkedSceneManager networkedSceneManager,
        NetworkedClientInitializer connectionInitializer
        )
    {
        _serverButton = serverButton;
        _joinButton = joinButton;
        _backButton = backButton;
        _serverManager = serverManager;
        _networkedSceneManager = networkedSceneManager;
        _initializer = connectionInitializer;
    }

    public void Initialize()
    {
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
        // TODO MG make this JoinServer independent of the input fields
        _initializer.JoinServer();
        // TODO MG
        // load this scene throguht networkedSceneManager
        // SceneManager.LoadScene(1);
    }

    public void JoinButtonClicked()
    {
        _initializer.JoinServer();
        // load this scene throguht networkedSceneManager
        //SceneManager.LoadScene(1);
    }

    public void BackButtonClicker()
    {
        SceneManager.LoadScene(0,LoadSceneMode.Single);
    }


}