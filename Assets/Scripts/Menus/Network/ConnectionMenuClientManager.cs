using DarkRift;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ConnectionMenuClientManager: IInitializable, IDisposable
{
    private NetworkRelay _networkRelay;

    public ConnectionMenuClientManager(
        NetworkRelay networkRelay)
    {
        _networkRelay = networkRelay;
    }

    public void Initialize()
    {
        _networkRelay.Subscribe(Tags.LoadLobby, ParseLoadLobbyMessage);
    }

    public void Dispose()
    {
        _networkRelay.Unsubscribe(Tags.LoadLobby, ParseLoadLobbyMessage);
    }

    private void ParseLoadLobbyMessage(Message message)
    {
        int sceneBuildIndex;
        using (DarkRiftReader reader = message.GetReader())
        {
            //read client id
            reader.ReadInt16();
            //read scene id
            sceneBuildIndex = reader.ReadInt16();
        }

        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
    }

   
}