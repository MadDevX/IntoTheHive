using DarkRift;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

/// <summary>
/// Scene-local connectionMenu only class used to handle messages as client on that scene
/// </summary>
public class ConnectionMenuClientMessageReceiver: IInitializable, IDisposable
{
    private NetworkRelay _networkRelay;

    public ConnectionMenuClientMessageReceiver(
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

    /// <summary>
    /// Parses a LoadLobby TagMessage
    /// </summary>
    /// <param name="message"></param>
    private void ParseLoadLobbyMessage(Message message)
    {
        int sceneBuildIndex;
        using (DarkRiftReader reader = message.GetReader())
        {
            // TODO MG CHECKSIZE
            //read client id
            reader.ReadInt16();
            //read scene id
            sceneBuildIndex = reader.ReadInt16();
        }

        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
    }

   
}