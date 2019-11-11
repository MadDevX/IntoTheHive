using DarkRift;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Zenject;

public class NetworkedSceneManager: IInitializable,IDisposable
{
    private Scenes _scenes;
    private NetworkRelay _relay;
    private Dictionary<ushort, SceneReference> _sceneReferences;
    public NetworkedSceneManager(
        Scenes scenes,
        NetworkRelay relay
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

    public void ParseChangeSceneMessage(Message message)
    {
        using (DarkRiftReader reader = message.GetReader())
        {
            // check message size 
            int sceneBuildIndex = (int)reader.ReadUInt16();
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
            // TODO MG - reply with acknowledgment?
        }
    }
}

