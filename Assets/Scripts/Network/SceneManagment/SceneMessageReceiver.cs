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

    public SceneMessageReceiver(
        Scenes scenes,
        NetworkRelay relay
        )
    {
        _scenes = scenes;
        _relay = relay;
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

        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
    }
}

