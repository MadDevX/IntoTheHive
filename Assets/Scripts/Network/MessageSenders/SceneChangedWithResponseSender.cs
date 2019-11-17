using DarkRift;
using DarkRift.Client.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SceneChangedWithResponseSender : IInitializable, IDisposable
{
    public event Action Completed;

    private NetworkRelay _relay;
    private UnityClient _client;
    private GlobalHostPlayerManager _playerManager;
    private Dictionary<ushort, bool> PlayersWithSceneReady;

    public SceneChangedWithResponseSender(
        NetworkRelay relay,
        UnityClient client,
        GlobalHostPlayerManager playerManager)
    {
        PlayersWithSceneReady = new Dictionary<ushort, bool>();
        _relay = relay;
        _client = client;
        _playerManager = playerManager;
    }

    public void Initialize()
    {
        _relay.Subscribe(Tags.SceneReady, HandleSceneReady);
    }

    public void Dispose()
    {
        _relay.Unsubscribe(Tags.SceneReady, HandleSceneReady);
    }

    public void SendSceneChangedWithResponse(int buildIndex)
    {
        
        ResetDictionary();

        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            ushort buildIndexUint = (ushort)buildIndex;
            writer.Write(buildIndexUint);

            using (Message message = Message.Create(Tags.ChangeSceneWithReply, writer))
            {
                _client.SendMessage(message, SendMode.Reliable);
                Debug.Log("Sent a message");
            }
        }
    }

    private void ResetDictionary()
    {
        PlayersWithSceneReady.Clear();
        
        foreach(ushort client in _playerManager.ConnectedPlayers)
        {
            PlayersWithSceneReady.Add(client, false);
        }
    }

    private void HandleSceneReady(Message message)
    {
        Debug.Log("Handle Scene ready");
        using (DarkRiftReader reader = message.GetReader())
        {
            //check size 
            ushort id = reader.ReadUInt16();
            if(PlayersWithSceneReady.ContainsKey(id))
            {
                PlayersWithSceneReady.Remove(id);
            }
            else
            {
                throw new ArgumentException("Received a message from client not present in the game");
            }
            this.PlayersWithSceneReady.Add(id, true);
            AreAllScenesReady();
        }
    }

    private void AreAllScenesReady()
    {
        bool allReady = PlayersWithSceneReady.Keys.Count > 0;
        foreach (ushort client in PlayersWithSceneReady.Keys)
        {
            allReady = allReady && PlayersWithSceneReady[client];
        }

        if(allReady)
        {
            Completed?.Invoke();
            Debug.Log("Loaded All scenes");
            foreach( var action in Completed.GetInvocationList())
            {
                // TODO MG : make sure all event are unsubsrcibed
                Completed -= (Action)action;
            }
        }

    }
}

