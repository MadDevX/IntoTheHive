using DarkRift.Client;
using DarkRift.Client.Unity;
using UnityEngine;

//Class which handles message receiving for networked characters
public class MessageReceiver
{
    public ushort ClientId { get; set; }
    public NetworkedCharacterInput _networkedCharacterMovement;
    public NetworkedCharacterShooting _networkedCharacterShooting;
    private UnityClient _networkManager;


    // placeholder class for message handling
    public MessageReceiver(
        UnityClient client,
        NetworkedCharacterInput networkedCharacterMovement,
        NetworkedCharacterShooting networkedCharacterShooting)
    {
        _networkManager = client;
        _networkedCharacterMovement = networkedCharacterMovement;
        _networkedCharacterShooting = networkedCharacterShooting;

        _networkManager.MessageReceived += HandleMessage;
    }

    private void HandleMessage(object sender, MessageReceivedEventArgs e)
    {
        Debug.Log("message received");
       // throw new System.NotImplementedException();
    }
}