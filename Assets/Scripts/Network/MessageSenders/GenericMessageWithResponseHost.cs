using DarkRift;
using DarkRift.Client.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

// TODO MG : Future idea - generalize class to provide a mechanism for any message type

/// <summary>
/// This class sends a SceneChanged message to all clients and executes a given action when all of the clients respond with a SceneReady message.
/// </summary>
public class GenericMessageWithResponseHost : IInitializable, IDisposable
{
    private event Action Completed;
    private Dictionary<ushort, bool> ReadyPlayers;
    private NetworkRelay _relay;
    private UnityClient _client;
    private GlobalHostPlayerManager _playerManager;

    public GenericMessageWithResponseHost(
        NetworkRelay relay,
        UnityClient client,
        SceneMessageSender sender,
        GlobalHostPlayerManager playerManager,
        ScenePostinitializationEvents postInitEvents)
    {
        ReadyPlayers = new Dictionary<ushort, bool>();
        _relay = relay;
        _client = client;
        _playerManager = playerManager;
    }

    public void Initialize()
    {
        _relay.Subscribe(Tags.ClientReady, HandleReadyMessage);
    }

    public void Dispose()
    {
        _relay.Unsubscribe(Tags.ClientReady, HandleReadyMessage);
    }

    public void SendMessageWithResponse(Message message, Action actionOnComplete = null, SendMode sendMode = SendMode.Reliable)
    {
        BuildClientsList();
        ClearEvents();
        if(actionOnComplete != null)
            Completed += actionOnComplete;        
        _client.SendMessage(message, sendMode);
    }

    private void BuildClientsList()
    {
        ReadyPlayers.Clear();
        foreach (ConnectedPlayerData client in _playerManager.ConnectedPlayers)
        {
            ReadyPlayers.Add(client.ID, false);
        }
    }

    private void HandleReadyMessage(Message message)
    {
        using (DarkRiftReader reader = message.GetReader())
        {
            
            //TODO MG CHECKSIZE
            ushort id = reader.ReadUInt16();
            
            if (ReadyPlayers.ContainsKey(id))
            {
                ReadyPlayers[id] = true;
            }
            else
            {
                Debug.Log(id);
                throw new ArgumentException("Received a message from client not present in the game");
            }
            AreAllClientsReady();
        }
    }

    private void AreAllClientsReady()
    {
        bool allReady = true;

        foreach (ushort client in ReadyPlayers.Keys)
        {
            allReady = allReady && ReadyPlayers[client];
        }

        if (allReady && ReadyPlayers.Keys.Count > 0)
        {
            Completed?.Invoke();
  
        }
    }

    private void ClearEvents()
    {
        if(Completed != null)
        {
            foreach (var action in Completed.GetInvocationList())
            {
                Completed -= (Action)action;
            }
        }
    }
}

