using DarkRift;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class NetworkedSceneManager: IInitializable,IDisposable
{
    private Scenes _scenes;
    private NetworkRelay _relay;
    private ChangeSceneMessageSender _messageSender;

    // TODO MG figure out if this scenes should be stored here somehow
    private Dictionary<ushort, SceneReference> _sceneReferences;

    public NetworkedSceneManager(
        Scenes scenes,
        NetworkRelay relay,
        ChangeSceneMessageSender messageSender
        )
    {
        _scenes = scenes;
        _relay = relay;
        _messageSender = messageSender;
        _sceneReferences = new Dictionary<ushort, SceneReference>();
    }

    public void Initialize()
    {
        _relay.Subscribe(Tags.ChangeScene, ParseChangeSceneMessage);
    }
    public void Dispose()
    {
        _relay.Unsubscribe(Tags.ChangeScene, ParseChangeSceneMessage);
    }

    public void ParseChangeSceneMessage(Message message)
    {
        int sceneBuildIndex;

        using (DarkRiftReader reader = message.GetReader())
        {
            reader.ReadBoolean();
            sceneBuildIndex = (int)reader.ReadUInt16();
        }

        var asyncOperation = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Single);
        asyncOperation.completed += SceneReady;
    }

    private void SceneReady(AsyncOperation obj)
    {
        _messageSender.SendSceneReady();
    }
}

