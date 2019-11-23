using DarkRift;
using DarkRift.Client.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// This class sends a SceneChanged message to all clients and executes a given action when all of the clients respond with a SceneReady message.
/// </summary>
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

    /// <summary>
    /// Send SceneChanged message to all clients and invoke an action after all recepients loaded their scenes and responded.
    /// Each call of this method clears previously subscribed events and requires no additional operations to work.
    /// </summary>
    /// <param name="buildIndex">BuildIndex of the scene to be loaded</param>
    /// <param name="actionOnComplete">Action to be invoked when all clients load the scene and respond</param>
    public void SendSceneChangedWithResponse(int buildIndex, Action actionOnComplete)
    {
        ResetDictionary();
        this.Completed += actionOnComplete;

        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            ushort buildIndexUint = (ushort)buildIndex;
            writer.Write(buildIndexUint);

            using (Message message = Message.Create(Tags.ChangeSceneWithReply, writer))
            {
                _client.SendMessage(message, SendMode.Reliable);
            }
        }
    }

    /// <summary>
    /// Resets the current dictionary with information who has a scene ready and who doesn't.
    /// New Dictionary is filled based on the ConnectedPlayers list from GlobalHostPlayerManager
    /// </summary>
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
        using (DarkRiftReader reader = message.GetReader())
        {
            //TODO MG CHECKSIZE
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

    /// <summary>
    /// Checks if all players sent a SceneReady message. 
    /// If that is the case, the action is invoked and all subscribed actions cleared.
    /// </summary>
    private void AreAllScenesReady()
    {
        bool allReady = true;

        foreach (ushort client in PlayersWithSceneReady.Keys)
        {
            allReady = allReady && PlayersWithSceneReady[client];
        }

        if(allReady && PlayersWithSceneReady.Keys.Count > 0)
        {
            Completed?.Invoke();
            ClearSubscribedActions();
        }

    }

    private void ClearSubscribedActions()
    {
        foreach (var action in Completed.GetInvocationList())
        {
            Completed -= (Action)action;
        }
    }

}

