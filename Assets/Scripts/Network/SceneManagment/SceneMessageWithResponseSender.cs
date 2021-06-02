using DarkRift;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

/// <summary>
/// This class sends a SceneChanged message to all clients and executes a given action when all of the clients respond with a SceneReady message.
/// </summary>
public class SynchronizedSceneManager : IInitializable, IDisposable
{
    private NetworkRelay _relay;
    private ScenePostinitializationEvents _postInitEvents;
    private GenericMessageWithResponseHost _messageWithResponseHost;
    private GenericMessageWithResponseClient _messageWithResponseClient;

    public SynchronizedSceneManager(
        NetworkRelay relay,
        ScenePostinitializationEvents postInitEvents,
        GenericMessageWithResponseHost messageWithResponseHost,
        GenericMessageWithResponseClient messageWithResponseClient)
    {
        _relay = relay;
        _postInitEvents = postInitEvents;
        _messageWithResponseHost = messageWithResponseHost;
        _messageWithResponseClient = messageWithResponseClient;
    }

    public void Initialize()
    {
        _relay.Subscribe(Tags.ChangeSceneWithReply, ParseMessage);
    }

    public void Dispose()
    {
        _relay.Unsubscribe(Tags.ChangeSceneWithReply, ParseMessage);
    }
    
    public void SendSceneChanged(int buildIndex, Action onComplete)
    {
        _messageWithResponseHost.SendMessageWithResponse(PrepareMessage(buildIndex), onComplete);
    }

    public void SendSceneChanged(int buildIndex)
    {
        _messageWithResponseHost.SendMessageWithResponse(PrepareMessage(buildIndex));
    }


    public Message PrepareMessage(int buildIndex)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            ushort buildIndexUint = (ushort)buildIndex;
            writer.Write(buildIndexUint);

            return Message.Create(Tags.ChangeSceneWithReply, writer);
        }
    }   

    private void ParseMessage(Message message)
    {
        int sceneBuildIndex;

        using (DarkRiftReader reader = message.GetReader())
        {
            sceneBuildIndex = reader.ReadUInt16();
        }

        // Loads a scene and Sends ClientReady when scene is loaded and properly initialized
        _postInitEvents.Subscribe(sceneBuildIndex, _messageWithResponseClient.SendClientReady);
        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);

    }
}

