using System;
using DarkRift;
using DarkRift.Client.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMessageSender
{
    private UnityClient _client;

    public SceneMessageSender(
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

    public void SendSceneChangedToPlayer(ushort clientId, ushort sceneBuildIndex)
    {        
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(clientId);
            writer.Write(sceneBuildIndex);

            using (Message message = Message.Create(Tags.LoadLobby, writer))
            {
                _client.SendMessage(message, SendMode.Reliable);
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
            writer.Write(_client.ID);
            Debug.Log("id wysylajacego : " + _client.ID);
            using (Message message = Message.Create(Tags.SceneReady, writer))
            {
                _client.SendMessage(message, SendMode.Reliable);
                Debug.Log("Sent sceneReeady");
            }
        }
    }
}