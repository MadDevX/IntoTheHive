using DarkRift;
using DarkRift.Client.Unity;

/// <summary>
/// This is a generic class responsible for answering a message with response if necessary
/// </summary>
public class GenericMessageWithResponseClient
{
    private UnityClient _client;

    public GenericMessageWithResponseClient(
        UnityClient client
        )
    {
        _client = client;
    }

    public void SendClientReady(ushort tag)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(_client.ID);
            writer.Write(tag);

            using (Message message = Message.Create(Tags.ClientReady, writer))
            {
                _client.SendMessage(message,SendMode.Reliable);
            }
        }
    }
}

