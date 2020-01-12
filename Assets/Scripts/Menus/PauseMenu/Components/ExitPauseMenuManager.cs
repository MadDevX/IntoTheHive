using DarkRift.Client.Unity;
using System;
using UnityEngine.UI;
using Zenject;

public class ExitPauseMenuManager: IInitializable, IDisposable
{
    private ServerManager _serverManager;
    private UnityClient _client;
    private ClientInfo _clientInfo;
    private Button _exitButton;
    public ExitPauseMenuManager(
        [Inject(Id =Identifiers.PauseExitButton)]
        Button exitButton,
        ServerManager serverManager,
        UnityClient client,
        ClientInfo clientInfo)
    {
        _exitButton = exitButton;
        _serverManager = serverManager;
        _client = client;
        _clientInfo = clientInfo;
    }
   

    public void Initialize()
    {
        _exitButton.onClick.AddListener(QuitGame);
    }

    public void Dispose()
    {
        _exitButton.onClick.RemoveListener(QuitGame);
    }

    private void QuitGame()
    {
        if(_clientInfo.Status == ClientStatus.Host)
        {
            _serverManager.CloseServer();
        }
        if (_clientInfo.Status == ClientStatus.Client)
        {
            _client.Disconnect();
        }
    }
}