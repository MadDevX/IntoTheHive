using System;
using DarkRift;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;


// This class handles message receival for host side of the lobby
public class HostLobbyManager: IInitializable, IDisposable
{
    public event Action<bool> AllPlayersReady;
    
    private LobbyState _lobbyState;
    private NetworkRelay _relay;
    private NetworkedSceneManager _sceneManager;
    private ChangeSceneMessageSender _sceneMessageSender;
    private LobbyMessageSender _lobbyMessageSender;
    private GlobalHostPlayerManager _globalHostPlayerManager; 

    public HostLobbyManager(
        NetworkRelay relay,
        LobbyState lobbyState,
        NetworkedSceneManager sceneManager,
        LobbyMessageSender lobbyMessageSender,
        ChangeSceneMessageSender sceneMessageSender,
        GlobalHostPlayerManager globalHostPlayerManager
        )
    {
        _relay = relay;
        _lobbyState = lobbyState;
        _sceneManager = sceneManager;
        _lobbyMessageSender = lobbyMessageSender;
        _sceneMessageSender = sceneMessageSender;
        _globalHostPlayerManager = globalHostPlayerManager; 
    }

    public void Initialize()
    {
        _relay.Subscribe(Tags.PlayerJoined, HandlePlayerJoined);
        _relay.Subscribe(Tags.RequestUpdateLobby, HandleRequestUpdateLobby);
        _relay.Subscribe(Tags.IsPlayerReady, HandleIsPlayerReady);

        foreach (ushort client in _globalHostPlayerManager.ConnectedPlayers)
        {
            _lobbyState.PlayersReadyStatus.Add(client, false);
        }
    }

    public void Dispose()
    {
        _relay.Unsubscribe(Tags.PlayerJoined, HandlePlayerJoined);
        _relay.Unsubscribe(Tags.RequestUpdateLobby, HandleRequestUpdateLobby);
        _relay.Unsubscribe(Tags.IsPlayerReady, HandleIsPlayerReady);
    }

    public void SetPlayerReady()
    {
        _lobbyMessageSender.SendIsPlayerReadyMessage(true);
    }

    // TODO MG Move this method elsewhere or add similiar in NetworkedSceneManager?
    private void AreAllPlayersReady()
    {
        bool allReady = true;
        foreach(bool value in _lobbyState.PlayersReadyStatus.Values)
        {
            allReady = allReady && value;
        }       

        AllPlayersReady?.Invoke(allReady);
    }

    private void HandleIsPlayerReady(Message message)
    {
        ushort id;
        bool isReady;
        using (DarkRiftReader reader = message.GetReader())
        {
            id = reader.ReadUInt16();
            Debug.Log(id);
            isReady = reader.ReadBoolean();
            Debug.Log(isReady);
        }

        if (_lobbyState.PlayersReadyStatus.ContainsKey(id))
        {
            _lobbyState.PlayersReadyStatus.Remove(id);
        }

        _lobbyState.PlayersReadyStatus.Add(id, isReady);
        AreAllPlayersReady();

        _lobbyMessageSender.SendUpdateLobbyMessage();
    }


    private void HandlePlayerJoined(Message message)
    {
        
        ushort id;
        //string name;
        using (DarkRiftReader reader = message.GetReader())
        {
            id = reader.ReadUInt16();
            //name = reader.ReadString();
        }

        _lobbyState.PlayersReadyStatus.Add(id, false);
        AreAllPlayersReady();
        
        // TODO MG : add some kind of sceneManager.GetSceneByName
        ushort sceneIndex = (ushort)2;
        
        _lobbyMessageSender.SendLoadLobbyMessage(id, sceneIndex);
    }

    private void HandleRequestUpdateLobby(Message message)
    {
        _lobbyMessageSender.SendUpdateLobbyMessage();
    }
}

