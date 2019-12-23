using System;
using DarkRift;
using UnityEngine;
using Zenject;

public class NetworkedCharacterInput : IInitializable, IDisposable
{
    private ControlState _controlState;
    private CharacterFacade _characterFacade;
    private NetworkRelay _networkRelay;

    public NetworkedCharacterInput(
        ControlState controlState,
        CharacterFacade characterFacade,
        NetworkRelay relay)
    {
        _controlState = controlState;
        _characterFacade = characterFacade;
        _networkRelay = relay;
    }

    public void Initialize()
    {
        _networkRelay.Subscribe(Tags.UpdateCharacterState, ParseMessage);
    }

    public void Dispose()
    {
        _networkRelay.Unsubscribe(Tags.UpdateCharacterState, ParseMessage);
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
                _controlState.PrimaryAction = primaryAction; //TODO: shooting should be handled directly, not through ControlState (unfortunately)
                _controlState.SecondaryAction = secondaryAction;
                _controlState.Direction = direction;
                _controlState.Position = position;
            }
        }
    }
    
}