using DarkRift;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

// TODO MG add to Lobby scene context
public class ClientLobbyManager: IInitializable, IDisposable
{
    private LobbyMessageSender _lobbyMessageSender;
    private NetworkRelay _networkRelay;

    public ClientLobbyManager(
        LobbyMessageSender lobbyMessageSender,
        NetworkRelay networkRelay)
    {
        _networkRelay = networkRelay;
        _lobbyMessageSender = lobbyMessageSender;
    }

    public void Initialize()
    {
        _networkRelay.Subscribe(Tags.LoadLobby, ParseLoadLobbyMessage);
        _networkRelay.Subscribe(Tags.UpdateLobby, ParseUpdateLobbyMessage);
    }

    public void Dispose()
    {
        _networkRelay.Unsubscribe(Tags.LoadLobby, ParseLoadLobbyMessage);
        _networkRelay.Unsubscribe(Tags.UpdateLobby, ParseUpdateLobbyMessage);
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

        var asyncAction = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Single);
        asyncAction.completed += RequestLobbyUpdate;
    }

    private void ParseUpdateLobbyMessage(Message message)
    {
        Debug.Log("Updated");
        using (DarkRiftReader reader = message.GetReader())
        {
            //checksize
            while(reader.Position < reader.Length)
            {
                ushort id = reader.ReadUInt16();
                bool ready = reader.ReadBoolean();
                // TODO MG : update info on Lobby menu fields to show it to players
            }
        }
    }

    private void RequestLobbyUpdate(AsyncOperation operation)
    {
        // Now Fires also for host
        _lobbyMessageSender.SendRequestLobbyUpdate();
        operation.completed -= RequestLobbyUpdate;
    }
}

