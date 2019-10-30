using DarkRift.Client;
using DarkRift.Client.Unity;

public class MessageReceiver
{
    UnityClient _client;
    NetworkedCharacterMovement _networkedCharacterMovement;
    NetworkedCharacterShooting _networkedCharacterShooting;

    // placeholder class for message handling
    public MessageReceiver(UnityClient client, NetworkedCharacterMovement networkedCharacterMovement, NetworkedCharacterShooting networkedCharacterShooting)
    {
        _client = client;
        _networkedCharacterMovement = networkedCharacterMovement;
        _networkedCharacterShooting = networkedCharacterShooting;

        _client.MessageReceived += HandleMessage;
    }

    private void HandleMessage(object sender, MessageReceivedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}