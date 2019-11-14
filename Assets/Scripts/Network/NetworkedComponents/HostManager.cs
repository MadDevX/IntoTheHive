using System;
using DarkRift;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;


//Divide this class
public class HostManager: IInitializable, IDisposable
{
    private NetworkRelay _relay;
    private NetworkedSceneManager _sceneManager;
    private ChangeSceneMessageSender _sceneMessageSender;

    public HostManager(
        NetworkRelay relay,
        NetworkedSceneManager sceneManager,
        ChangeSceneMessageSender sceneMessageSender
        )
    {
        _relay = relay;
        _sceneManager = sceneManager;
        _sceneMessageSender = sceneMessageSender;
    }

    public void Initialize()
    {
        // TODO MG use request Host Scene as message that anounces arrival to lobby or make a new message
        // TODO MG change requestHostScene to "PlayerJoined" and ApplyHostScene to "LobbyStateUpdate"
        _relay.Subscribe(Tags.PlayerJoined, HandlePlayerJoined);
        _relay.Subscribe(Tags.SceneReady, HandleSceneReady);
    }

    public void Dispose()
    {
        _relay.Unsubscribe(Tags.PlayerJoined, HandlePlayerJoined);
        _relay.Unsubscribe(Tags.SceneReady, HandleSceneReady);
    }

    public void LoadNextLevel()
    {
        // Send next level message to everybody in the form of SceneChangedMessage
    }

    private void HandlePlayerJoined(Message message)
    {
        Debug.Log("Request for the host scene handled");
        ushort id;
        using (DarkRiftReader reader = message.GetReader())
        {
            id = reader.ReadUInt16();
        }
        ushort sceneIndex = (ushort)SceneManager.GetActiveScene().buildIndex;
        _sceneMessageSender.SendApplyHostScene(id, sceneIndex);
    }

    private void HandleSceneReady(Message message)
    {
        throw new NotImplementedException();
    }

}

