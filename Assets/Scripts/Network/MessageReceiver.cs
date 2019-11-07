using System;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using UnityEngine;

//Class which handles message receiving for networked characters
public class MessageReceiver
{
    public ushort ClientId { get; set; }
    public NetworkedCharacterInput _networkedCharacterInput;
    public NetworkedCharacterEquipment _networkedCharacterEquipment;
    private UnityClient _networkManager;


    // placeholder class for message handling
    public MessageReceiver(
        UnityClient client,
        NetworkedCharacterInput networkedCharacterMovement,
        NetworkedCharacterEquipment networkedCharacterShooting
        )
    {
        _networkManager = client;
        _networkedCharacterInput = networkedCharacterMovement;
        _networkedCharacterEquipment = networkedCharacterShooting;
        _networkManager.MessageReceived += MessageReceivedHandler;
    }

    private void MessageReceivedHandler(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            if(message.Tag == Tags.UpdateCharacterState)
            {
                HandleUpdateState(message);
            }
            if(message.Tag == Tags.UpdateCharacterEquipment)
            {
                HandleUpdateEquipment(message);
            }
        }
    }

    private void HandleUpdateState(Message message)
    {
        // TODO MG check message size 
        using (DarkRiftReader reader = message.GetReader())
        {
            ushort clientId = reader.ReadUInt16();
            if(clientId == this.ClientId)
            {
                float vertical = reader.ReadSingle();
                float horizontal = reader.ReadSingle();
                bool primaryAction = reader.ReadBoolean();
                bool secondaryAction = reader.ReadBoolean();

                float directionX = reader.ReadSingle();
                float directionY = reader.ReadSingle();
                Vector2 direction = new Vector2(directionX, directionY);

                float positionX = reader.ReadSingle();
                float positionY = reader.ReadSingle();
                Vector2 position = new Vector2(positionX, positionY);

                _networkedCharacterInput.SetAxes(vertical, horizontal);
                _networkedCharacterInput.SetActions(primaryAction,secondaryAction);
                _networkedCharacterInput.SetDirection(direction);
                _networkedCharacterInput.SetPosition(position);
            }    
        }
    }
    private void HandleUpdateEquipment(Message message)
    {
        // TODO MG check message size 
        using (DarkRiftReader reader = message.GetReader())
        {
            ushort clientId = reader.ReadUInt16();
            if (clientId == this.ClientId)
            {
                // TODO How to store data about weapons
                // do something with NetworkedCharacterEquipment
                // read content
                object arguments = null;
                _networkedCharacterEquipment.CreateWeapon(arguments);

            }
        }
    }
}