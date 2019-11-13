using System;
using DarkRift;
using DarkRift.Client.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneMessageSender
{
    private UnityClient _client;

    public ChangeSceneMessageSender(
        UnityClient client)
    {
        _client = client;
    }

    public void SendSceneChanged(ushort sceneBuildIndex)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(sceneBuildIndex);

            using (Message message = Message.Create(Tags.ChangeScene, writer))
            {
                _client.SendMessage(message,SendMode.Reliable);
            }
        }
    }

    public void SendSceneChanged(string sceneName)
    {
        var scene = SceneManager.GetSceneByName(sceneName);
        SendSceneChanged((ushort)scene.buildIndex);
    }
    
    public void SendSceneReady()
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(_client.Client.ID);

            using (Message message = Message.Create(Tags.SceneReady, writer))
            {
                _client.SendMessage(message, SendMode.Reliable);
            }
        }
    }

    public void SendApplyHostScene(ushort id, ushort sceneIndex)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(id);
            writer.Write(sceneIndex);
            using (Message message = Message.Create(Tags.ApplyHostScene, writer))
            {
                _client.SendMessage(message, SendMode.Reliable);
            }
        }
    }

    public void RequestHostScene()
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(_client.ID);
            using (Message message = Message.Create(Tags.RequestHostScene, writer))
            {
                Debug.Log("Sent request");
                _client.SendMessage(message, SendMode.Reliable);
            }
        }
    }
}