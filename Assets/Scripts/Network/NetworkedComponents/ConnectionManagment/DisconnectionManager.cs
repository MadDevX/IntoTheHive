using DarkRift.Client.Unity;
using System;
using UnityEngine.SceneManagement;
using Zenject;

public class DisconnectionManager: IInitializable, IDisposable
{
    private UnityClient _client;

    public DisconnectionManager(
        UnityClient client)
    {
        _client = client;
    }


    public void Initialize()
    {
        _client.Disconnected += HandleDisconnect;
    }

    public void Dispose()
    {
        _client.Disconnected -= HandleDisconnect;
    }

    private void HandleDisconnect(object sender, DarkRift.Client.DisconnectedEventArgs e)
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}

