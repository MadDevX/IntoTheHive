using DarkRift.Client.Unity;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// This class gathers button handlers from Lobby Menu 
/// It uses networking classes to set player status as ready and start the game
/// </summary>
public class LobbyMenuManager: IInitializable, IDisposable
{
    private bool _isPlayerReady;
    private Button _startGameButton;
    private Button _readyButton;
    private Button _leaveLobbyButton;

    private UnityClient _client;
    private ClientInfo _clientInfo;
    private ServerManager _serverManager;
    private LobbyMessageSender _messageSender;
    private ProjectEventManager _eventManager;
    private LobbyStateManager _lobbyStateManager;
    private SynchronizedSceneManager _synchronizedSceneManager;

    public LobbyMenuManager(
        [Inject(Id = Identifiers.LobbyStartGameButton)]
        Button startGameButton,
        [Inject(Id = Identifiers.LobbyReadyButton)]
        Button readyButton,
        [Inject(Id = Identifiers.LobbyLeaveButton)]
        Button leaveButton,
        UnityClient client,
        ClientInfo clientInfo,
        ServerManager serverManager,
        LobbyMessageSender messageSender,
        ProjectEventManager eventManager,
        LobbyStateManager lobbyStateManager,
        NetworkedCharacterSpawner characterSpawner,
        SynchronizedSceneManager synchronizedSceneManager
        )
    {
        _readyButton = readyButton;
        _leaveLobbyButton = leaveButton;
        _startGameButton = startGameButton;

        _client = client;
        _clientInfo = clientInfo;
        _eventManager = eventManager;
        _serverManager = serverManager;
        _messageSender = messageSender;
        _lobbyStateManager = lobbyStateManager;
        _synchronizedSceneManager = synchronizedSceneManager;
    }

    public void Initialize()
    {
        _startGameButton.onClick.AddListener(StartGame);
        _readyButton.onClick.AddListener(SetReadyStatus);
        _leaveLobbyButton.onClick.AddListener(LeaveLobby);

        // Disable non-host elements
        if (_clientInfo.Status != ClientStatus.Host)
        {
            DisableHostFunctionality();
        }

        SetupReadyHandlers();
    }

    public void Dispose()
    {
        _startGameButton.onClick.RemoveListener(StartGame);
        _readyButton.onClick.RemoveListener(SetReadyStatus);
        _leaveLobbyButton.onClick.RemoveListener(LeaveLobby);
        _lobbyStateManager.AllPlayersReadyChanged -= StartGameButtonSetActive;
    }  

    /// <summary>
    /// Starts the game by sending all players the ChangeScene message and spawning players when all clients are ready.
    /// </summary>
    public void StartGame()
    {
        _synchronizedSceneManager.SendSceneChanged(3, _eventManager.FireGameInitializedHost);
    }

    /// <summary>
    /// Sends a message to the host to update the client's ready status.
    /// </summary>
    public void SetReadyStatus()
    {
        _isPlayerReady = !_isPlayerReady;
        _messageSender.SendIsPlayerReadyMessage(_isPlayerReady);
    }

    public void LeaveLobby()
    {
        if (_clientInfo.Status == ClientStatus.Host)
        {
            // TODO MG : Send host left the game message 
            _serverManager.CloseServer();
            // Should this also disconnect the host client or should he react to "disconnected event"?
        }

        if (_clientInfo.Status == ClientStatus.Client)
        {
            _client.Disconnect();
        }

        SceneManager.LoadScene("ConnectionMenu");
    }

    /// <summary>
    /// This method disables start game button initially and makes it clickable when all players are active
    /// </summary>
    private void SetupReadyHandlers()
    {
        _startGameButton.interactable = false;
        _lobbyStateManager.AllPlayersReadyChanged += StartGameButtonSetActive;
    }

    /// <summary>
    /// Enables StartGame button when all players ale ready. Otherwise disables it.
    /// </summary>
    /// <param name="isActive"></param>
    private void StartGameButtonSetActive(bool isActive)
    {
        _startGameButton.interactable = isActive;
    }

    /// <summary>
    /// This fuction disables the host functionality existing in the lobby
    /// i.e. disables 'start game' button etc.
    /// </summary>
    private void DisableHostFunctionality()
    {
        _startGameButton.gameObject.SetActive(false);
    }

}