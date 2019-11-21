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
}