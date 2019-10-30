using DarkRift;
using DarkRift.Client.Unity;

/// <summary>
/// Scene-local connectionMenu only class used to send messages on that scene
/// </summary>
public class ConnectionMenuMessageSender
{
    private UnityClient _client;
    private SceneMessageSender _sceneMessageSender;

    public ConnectionMenuMessageSender(
        UnityClient client,
        SceneMessageSender sceneMessageSender)
    {
        _client = client;
        _sceneMessageSender = sceneMessageSender;
    }

    /// <summary>
    /// Sends a PlayerJoined message. 
    /// </summary>
    public void SendPlayerJoinedMessage()
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(_client.ID);
            using (Message message = Message.Create(Tags.PlayerJoined, writer))
            {
                _client.SendMessage(message, SendMode.Reliable);
            }
            //LoadLobbyMessage is sent as response
        }
    }

    /// <summary>
    /// Sends a LoadLobby message to a given player. 
    /// </summary>
    /// <param name="id"> Id of client who should change the scene </param>
    /// <param name="sceneIndex">Index of the scene which </param>
    public void SendLoadLobbyMessage(ushort id, ushort sceneIndex)
    {
        _sceneMessageSender.SendSceneChangedToPlayer(id, sceneIndex);
    }

    /// <summary>
    /// Sends a message with a request to receive a list of current players in the lobby along with their ready status.
    /// </summary>
    public void SendRequestLobbyUpdate()
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            using (Message message = Message.Create(Tags.RequestUpdateLobby, writer))
            {
                _client.SendMessage(message, SendMode.Reliable);
            }
        }
    }
}