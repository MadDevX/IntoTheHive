using DarkRift;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneMessageReceiver: IInitializable,IDisposable
{
    private Scenes _scenes;
    private NetworkRelay _relay;

    // TODO MG figure out if this scenes should be stored here somehow
    private Dictionary<ushort, SceneReference> _sceneReferences;

    public SceneMessageReceiver(
        Scenes scenes,
        NetworkRelay relay,
        SceneMessageSender messageSender
        )
    {
        _scenes = scenes;
        _relay = relay;
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

    private void ParseChangeSceneMessage(Message message)
    {
        int sceneBuildIndex;

        using (DarkRiftReader reader = message.GetReader())
        {
            sceneBuildIndex = reader.ReadUInt16();
        }

        var asyncOperation = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Single);
    }
}

