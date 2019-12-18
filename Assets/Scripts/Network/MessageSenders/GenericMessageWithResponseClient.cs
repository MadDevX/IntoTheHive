using DarkRift;
using DarkRift.Client.Unity;
using UnityEngine;

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

    public void SendClientReady()
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(_client.ID);

            using (Message message = Message.Create(Tags.ClientReady, writer))
            {
                _client.SendMessage(message,SendMode.Reliable);
            }
        }
    }
}

