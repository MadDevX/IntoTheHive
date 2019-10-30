using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using UnityEngine;
using Zenject;

/// <summary>
/// This class reacts to incoming messages and relays them to their corresponding handlers.
/// </summary>
public class NetworkRelay: IInitializable, IDisposable
{
    private Dictionary<ushort, List<Action<Message>>> _messageHandlers;
    private UnityClient _client;

    public NetworkRelay(
        UnityClient client)
    {
        _client = client;

        _messageHandlers = new Dictionary<ushort, List<Action<Message>>>();
        InitializeMessageHandlers();
    }

    public void Initialize()
    {
        _client.MessageReceived += HandleMessage;
    }


    public void Dispose()
    {
        _client.MessageReceived -= HandleMessage;
    }

    /// <summary>
    /// Initializes a list for each scene tag in 'Tags' class.
    /// </summary>
    private void InitializeMessageHandlers()
    {
        Type TagsType = typeof(Tags);
        var fieldsInfo = TagsType.GetFields().Where(Type => Type.FieldType == typeof(ushort));

        // Use reflection to initialize all message handlers lists in a generic way.
        foreach (FieldInfo field in fieldsInfo)
        {
            ushort tag = (ushort)field.GetValue(null);
            _messageHandlers.Add(tag, new List<Action<Message>>());
        }
    }

    /// <summary>
    /// Handles a message by invoking all handlers with a given message tag.
    /// </summary>
    /// <param name="sender">Sender of the message.</param>
    /// <param name="e">MessageReceivedEventArgs</param>
    private void HandleMessage(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            ushort tag = message.Tag;

            List<Action<Message>> list;
            _messageHandlers.TryGetValue(tag, out list);

            foreach(Action<Message> action in list)
            {
                action?.Invoke(message);
            }

        }
    }

    /// <summary>
    /// Subscribe an Action as a handler for a message with a given tag.
    /// </summary>
    /// <param name="tag">Tag of the message.</param>
    /// <param name="handlerMethod">Action that will handle the message.</param>
    public void Subscribe(ushort tag, Action<Message> handlerMethod)
    {
        List<Action<Message>> list;
        _messageHandlers.TryGetValue(tag, out list);
        list.Add(handlerMethod);
    }

    /// <summary>
    /// Unsubscribe an Action that is as a handler for a message with a given tag.
    /// </summary>
    /// <param name="tag">Tag of the message.</param>
    /// <param name="handlerMethod">Action that handles the message.</param>
    public void Unsubscribe(ushort tag, Action<Message> handlerMethod)
    {
        List<Action<Message>> list;
        _messageHandlers.TryGetValue(tag, out list);
        list.Remove(handlerMethod);
    }

}