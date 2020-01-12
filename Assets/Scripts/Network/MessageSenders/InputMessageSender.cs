using DarkRift;
using DarkRift.Client.Unity;
using GameLoop;
using UnityEngine;

class InputMessageSender : UpdatableObject
{
    private UnityClient _client;
    private ControlState _controlState;
    private CharacterFacade _characterFacade;
    private Settings _settings;

    private Vector2 _previousPosition;
    private Vector2 _previousRotation;
    private bool _previousPAction;
    private bool _previousSAction;

    private float _timeSinceLastSend;

    public InputMessageSender(
        UnityClient client,
        ControlState controlState,
        CharacterFacade characterFacade,
        Settings settings)
    {
        _client = client;
        _characterFacade = characterFacade;
        _controlState = controlState;
        _settings = settings;

        UpdatePreviousStates();
    }

    public override void OnUpdate(float deltaTime)
    {
        // Add RotationChange Detection / fire detection 
        float moveDifference = (_controlState.Position - _previousPosition).sqrMagnitude;
        float rotationDifference = (_controlState.Direction - _previousRotation).sqrMagnitude;

        if ((_timeSinceLastSend >= _settings.messageSenderDelay &&  
               (moveDifference >= _settings.moveEps * _settings.moveEps
                || rotationDifference >= _settings.rotationEps * _settings.rotationEps))
           || _controlState.PrimaryAction != _previousPAction
           || _controlState.SecondaryAction != _previousSAction)
        {
            _timeSinceLastSend = 0.0f;
            SendControlStateChangedMessage();
            UpdatePreviousStates();
        }
        _timeSinceLastSend += deltaTime;
    }

    public void SendControlStateChangedMessage()
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {

            writer.Write(_characterFacade.Id);

            writer.Write(_controlState.Vertical);
            writer.Write(_controlState.Horizontal);

            writer.Write(_controlState.PrimaryAction);
            writer.Write(_controlState.SecondaryAction);

            writer.Write(_controlState.Direction.x);
            writer.Write(_controlState.Direction.y);

            writer.Write(_controlState.Position.x);
            writer.Write(_controlState.Position.y);

            using (Message message = Message.Create(Tags.UpdateCharacterState, writer))
            {
                _client.SendMessage(message, SendMode.Unreliable);
            }
        }
    }

    private void UpdatePreviousStates()
    {
        _previousPosition = _controlState.Position;
        _previousRotation = _controlState.Direction;
        _previousPAction = _controlState.PrimaryAction;
        _previousSAction = _controlState.SecondaryAction;
    }

    [System.Serializable]
    public class Settings
    {
        public float moveEps = 0.001f;
        public float rotationEps = 0.1f;
        public float messageSenderDelay = 0.1f;
    }

}

