using DarkRift.Client.Unity;
using System;
using TMPro;
using UnityEngine.UI;
using Zenject;
/// <summary>
/// Manages Logic in GameEndedMenu. Displaying right text, disconnecting and chagning scenes on host sides
/// </summary>
public class GameEndedMenuManager: IInitializable, IDisposable
{
    private Button _okHostButton;
    private Button _leaveServerButton;
    private TextMeshProUGUI _loseText;
    private TextMeshProUGUI _winText;
    private ClientInfo _clientInfo;   
    private ServerManager _serverManager;
    private HostSceneManager _sceneManager;

    public GameEndedMenuManager(
        [Inject(Id = Identifiers.GameEndedHostOkButton)]
        Button okHostButton,
        [Inject(Id = Identifiers.GameEndedLeaveServerButton)]
        Button leaveServerButton,
        [Inject(Id = Identifiers.GameEndedLoseText)]
        TextMeshProUGUI loseText,
        [Inject(Id = Identifiers.GameEndedWinText)]
        TextMeshProUGUI winText,
        ClientInfo clientInfo,
        ServerManager serverManager,
        HostSceneManager sceneManager)
    {
        _okHostButton = okHostButton;
        _loseText = loseText;
        _leaveServerButton = leaveServerButton;
        _winText = winText;
        _clientInfo = clientInfo;
        _serverManager = serverManager;
        _sceneManager = sceneManager;
    }

    public void Initialize()
    {
        _okHostButton.onClick.AddListener(ReturnToLobby);
        _leaveServerButton.onClick.AddListener(LeaveServer);
        EnableFuncionality(_clientInfo.Status == ClientStatus.Host);            
    }

    public void Dispose()
    {
        _okHostButton.onClick.RemoveListener(ReturnToLobby);
        _leaveServerButton.onClick.AddListener(LeaveServer);
    }

    /// <summary>
    /// Sets the functionality of the menu wheter the client is a host or not.
    /// </summary>
    /// <param name="isHost"></param>
    private void EnableFuncionality(bool isHost)
    {
        _okHostButton.gameObject.SetActive(isHost);
        _winText.enabled = false;
        _loseText.enabled = false;
    }  

    /// <summary>
    /// Return to the lobby
    /// </summary>
    private void ReturnToLobby()
    {
        _sceneManager.LoadLobby();
    }

    /// <summary>
    /// Disconnects the client from the server or closes it.
    /// </summary>
    private void LeaveServer()
    {
        if(_clientInfo.Status == ClientStatus.Client)
            _clientInfo.Client.Disconnect();
        if (_clientInfo.Status == ClientStatus.Host)
            _serverManager.CloseServer();
    }
}