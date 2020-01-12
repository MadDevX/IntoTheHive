using System;
using DarkRift;
using GameLoop;
using UnityEngine;
using Zenject;

public class NetworkedCharacterInput : FixedUpdatableObject
{
    private ControlState _controlState;
    private CharacterFacade _characterFacade;
    private NetworkRelay _networkRelay;
    private Settings _settings;
    private CharacterMovement _movement;

    private float _timer;

    public NetworkedCharacterInput(
        ControlState controlState,
        CharacterFacade characterFacade,
        NetworkRelay relay,
        Settings settings,
        CharacterMovement movement)
    {
        _controlState = controlState;
        _characterFacade = characterFacade;
        _networkRelay = relay;
        _settings = settings;
        _movement = movement;
    }

    public override void Initialize()
    {
        base.Initialize();
        _networkRelay.Subscribe(Tags.UpdateCharacterState, ParseMessage);
    }

    public override void Dispose()
    {
        base.Dispose();
        _networkRelay.Unsubscribe(Tags.UpdateCharacterState, ParseMessage);
    }

    public override void OnFixedUpdate(float deltaTime)
    {
        _timer += deltaTime;
        if (_timer < _settings.stopExtrapolationDelay)
        {
            _controlState.Position = _movement.UpdatePosition(_controlState.Position, deltaTime);
        }
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
                _controlState.PrimaryAction = false;
                _controlState.SecondaryAction = secondaryAction;
                _controlState.Direction = direction;
                _controlState.Position = position;
            }
        }
        _timer = 0.0f;
    }

    [System.Serializable]
    public class Settings
    {
        public float stopExtrapolationDelay;
    }
}