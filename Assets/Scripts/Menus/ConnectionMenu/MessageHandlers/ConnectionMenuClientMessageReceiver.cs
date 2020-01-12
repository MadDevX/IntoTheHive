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
    private ConnectionMenuMessageSender _sender;
    private ScenePostinitializationEvents _postInitEvents;

    public ConnectionMenuClientMessageReceiver(
        NetworkRelay networkRelay,
        ConnectionMenuMessageSender sender,
        ScenePostinitializationEvents postInitEvents)
    {
        _sender = sender;
        _networkRelay = networkRelay;
        _postInitEvents = postInitEvents;
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
            //read client id
            reader.ReadInt16();
            //read scene id
            sceneBuildIndex = reader.ReadInt16();
        }

        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        _postInitEvents.Subscribe(sceneBuildIndex, RequestLobbyUpdate);
    }

    private void RequestLobbyUpdate()
    {
        _sender.SendRequestLobbyUpdate();
    }
   
}