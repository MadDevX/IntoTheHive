using DarkRift.Client.Unity;
using System;
using UnityEngine.SceneManagement;
using Zenject;

public class DisconnectionManager: IInitializable, IDisposable
{
    private UnityClient _client;
    private GameCycle _gameCycle;
    public DisconnectionManager(
        UnityClient client,
        GameCycle gameCycle)
    {
        _client = client;
        _gameCycle = gameCycle;
    }


    public void Initialize()
    {
        _client.Disconnected += HandleDisconnected;
    }

    public void Dispose()
    {
        _client.Disconnected -= HandleDisconnected;
    }

    private void HandleDisconnected(object sender, DarkRift.Client.DisconnectedEventArgs e)
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        _gameCycle.RaiseOnGameEnded();
    }
}

