using DarkRift;
using UnityEngine;

public class NetworkedCharacterInput
{ 
    private ControlState _controlState;
    //Use this class to modify Control state which will be used to conrtol the character through the Character Movement
    
    public NetworkedCharacterInput(ControlState controlState)
    {
        _controlState = controlState;
    }

    public void SetAxes(float vertical, float horizontal)
    {
        _controlState.Vertical = vertical;
        _controlState.Horizontal = horizontal;
    }

    public void SetDirection(Vector2 direction)
    {
        _controlState.Direction = direction;
    }

    public void SetPosition(Vector2 position)
    {
        _controlState.Position = position;
    }

    public void SetActions(bool primaryAction, bool secondaryAction)
    {
        _controlState.PrimaryAction = primaryAction;
        _controlState.SecondaryAction = secondaryAction;
    }
}