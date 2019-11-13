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

    public void SendSceneChangedClientsOnly(ushort sceneBuildIndex)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(sceneBuildIndex);

            using (Message message = Message.Create(Tags.ChangeSceneClientsOnly, writer))
            {
                _client.SendMessage(message, SendMode.Reliable);
            }
        }
    }

    public void SendSceneChangedClientsOnly(string sceneName)
    {
        var scene = SceneManager.GetSceneByName(sceneName);
        SendSceneChangedClientsOnly((ushort)scene.buildIndex);
    }
}