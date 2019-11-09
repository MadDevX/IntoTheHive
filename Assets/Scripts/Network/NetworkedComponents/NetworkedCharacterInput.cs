using System;
using DarkRift;
using UnityEngine;

public class NetworkedCharacterInput
{
    private ControlState _controlState;
    private CharacterFacade _characterFacade;

    public NetworkedCharacterInput(
        ControlState controlState,
        CharacterFacade characterFacade,
        NetworkRelay relay)
    {
        _controlState = controlState;
        _characterFacade = characterFacade;

        relay.Subscribe(Tags.DespawnCharacter, ParseMessage);
    }

    public void ParseMessage(Message message)
    {
        using (DarkRiftReader reader = message.GetReader())
        {

            ushort incomingClientId = reader.ReadUInt16();
            if (incomingClientId == _characterFacade.Id)
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

                _controlState.Vertical = vertical;
                _controlState.Horizontal = horizontal;
                _controlState.PrimaryAction = primaryAction;
                _controlState.SecondaryAction = secondaryAction;
                _controlState.Direction = direction;
                _controlState.Position = position;
            }
        }
    }
    
}