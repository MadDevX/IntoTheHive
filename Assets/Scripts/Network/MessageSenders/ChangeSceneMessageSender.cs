using DarkRift;
using DarkRift.Client.Unity;
using UnityEngine.SceneManagement;

public class ChangeSceneMessageSender
{
    private UnityClient _client;

    public ChangeSceneMessageSender(
        UnityClient client)
    {
        _client = client;
    }

    public void SendSceneChanged(ushort sceneBuildIndex, bool clientsOnly)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(clientsOnly);
            writer.Write(sceneBuildIndex);

            using (Message message = Message.Create(Tags.ChangeScene, writer))
            {
                _client.SendMessage(message,SendMode.Reliable);
            }
        }
    }

    public void SendSceneChanged(string sceneName, bool clientsOnly)
    {
        var scene = SceneManager.GetSceneByName(sceneName);
        SendSceneChanged((ushort)scene.buildIndex, clientsOnly);
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
}