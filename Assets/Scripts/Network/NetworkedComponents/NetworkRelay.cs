using System;
using System.Collections.Generic;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using Zenject;

public class NetworkRelay: IInitializable, IDisposable
{
    private Dictionary<ushort, List<Action<Message>>> _messageHandlers;
    private UnityClient _client;

    public NetworkRelay(
        UnityClient client)
    {
        _client = client;

        // TODO MG: REMOVE THIS SOLUTION ASAP
        // PLACEHOLDER TO ALLOW OTHER FUNCTIONS TO USE THIS CLASS
        // USE Event subscription
        _messageHandlers = new Dictionary<ushort, List<Action<Message>>>();
        _messageHandlers.Add(Tags.DespawnCharacter, new List<Action<Message>>());
        _messageHandlers.Add(Tags.SpawnCharacter, new List<Action<Message>>());
        _messageHandlers.Add(Tags.UpdateCharacterEquipment, new List<Action<Message>>());
        _messageHandlers.Add(Tags.UpdateCharacterState, new List<Action<Message>>());
        _messageHandlers.Add(Tags.ChangeScene, new List<Action<Message>>());
        _messageHandlers.Add(Tags.GameStarted, new List<Action<Message>>());
        _messageHandlers.Add(Tags.SceneReady, new List<Action<Message>>());
        _messageHandlers.Add(Tags.ConnectionInfo, new List<Action<Message>>());
        _messageHandlers.Add(Tags.PlayerJoined, new List<Action<Message>>());
        _messageHandlers.Add(Tags.UpdateLobby, new List<Action<Message>>());
        _messageHandlers.Add(Tags.RequestUpdateLobby, new List<Action<Message>>());
        _messageHandlers.Add(Tags.PlayerDisconnected, new List<Action<Message>>());
        _messageHandlers.Add(Tags.LoadLobby, new List<Action<Message>>());
        _messageHandlers.Add(Tags.IsPlayerReady, new List<Action<Message>>());
    }

    public void Initialize()
    {
        _client.MessageReceived += HandleMessage;
    }

    public void Dispose()
    {
        _client.MessageReceived -= HandleMessage;
    }

    private void HandleMessage(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            ushort tag = message.Tag;

            // Again - temporary solution
            List<Action<Message>> list;
            _messageHandlers.TryGetValue(tag, out list);

            foreach(Action<Message> action in list)
            {
                action?.Invoke(message);
            }

        }
    }

    public void Subscribe(ushort tag, Action<Message> handlerMethod)
    {
        // Again - temporary implementation
        List<Action<Message>> list;
        _messageHandlers.TryGetValue(tag, out list);
        list.Add(handlerMethod);
    }


    public void Unsubscribe(ushort tag, Action<Message> handlerMethod)
    {
        // Again - temporary implementation
        List<Action<Message>> list;
        _messageHandlers.TryGetValue(tag, out list);
        list.Remove(handlerMethod);
    }

}