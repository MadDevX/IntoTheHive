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
public class SceneMessageReceiverWithResponse : IInitializable, IDisposable
{
    public event Action Completed;

    private NetworkRelay _relay;
    private UnityClient _client;
    private SceneMessageSender _messageSender;
    private GlobalHostPlayerManager _playerManager;
    private ScenePostinitializationEvents _postInitEvents;

    private Dictionary<ushort, bool> PlayersWithSceneReady;

    public SceneMessageReceiverWithResponse(
        NetworkRelay relay,
        UnityClient client,
        SceneMessageSender sender,
        GlobalHostPlayerManager playerManager,
        ScenePostinitializationEvents postInitEvents)
    {
        PlayersWithSceneReady = new Dictionary<ushort, bool>();
        _relay = relay;
        _client = client;
        _messageSender = sender;
        _playerManager = playerManager;
        _postInitEvents = postInitEvents;
    }

    public void Initialize()
    {
        _relay.Subscribe(Tags.ChangeSceneWithReply, ParseChangeSceneWithResponseMessage);
    }

    public void Dispose()
    {
        _relay.Unsubscribe(Tags.ChangeSceneWithReply, ParseChangeSceneWithResponseMessage);
    }

    /// <summary>
    /// Sends a reply notifying that a scene is ready. Most probably located in sceneInitializedHandler
    /// </summary>
    public void SceneReady()
    {
        _messageSender.SendSceneReady();
    }

    /// <summary>
    /// Parses the original message sent by host.
    /// </summary>
    /// <param name="message">A ChangeSceneWithResponse message</param>
    private void ParseChangeSceneWithResponseMessage(Message message)
    {
        int sceneBuildIndex;

        using (DarkRiftReader reader = message.GetReader())
        {
            sceneBuildIndex = reader.ReadUInt16();
        }

        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        // Sends SceneReady when scene is loaded and properly initialized
        // TODO MG : REMEMBER TO UNSUBSCRIBE
        _postInitEvents.Subscribe(sceneBuildIndex, SceneReady);
    }
}

