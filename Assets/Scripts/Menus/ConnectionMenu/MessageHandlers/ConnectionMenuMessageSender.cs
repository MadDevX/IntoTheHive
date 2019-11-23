using DarkRift;
using DarkRift.Client.Unity;

public class ConnectionMenuMessageSender
{
    private UnityClient _client;

    public ConnectionMenuMessageSender(
        UnityClient client)
    {
        _client = client;
    }

    // this function is exactly the same in LobbyMessage Sender - make some project wide Message composer class
    public void PlayerJoinedMessage()
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(_client.ID);
            using (Message message = Message.Create(Tags.PlayerJoined, writer))
            {
                _client.SendMessage(message, SendMode.Reliable);
            }
        }
    }

    // this function is exactly the same in LobbyMessage Sender - make some project wide Message composer class
    // TODO MG create some kind of message composer 
    public void SendLoadLobbyMessage(ushort id, ushort sceneIndex)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(id);
            writer.Write(sceneIndex);
            using (Message message = Message.Create(Tags.LoadLobby, writer))
            {
                _client.SendMessage(message, SendMode.Reliable);
            }
        }
    }


}