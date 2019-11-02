using UnityEngine;

public class NetworkedCharacterInput
{
    //Rename this class to NetworkedCharacterInputReader?
    
    private ControlState _controlState;
    //Use this class to modify Control state which will be used to conrtol the character through the Character Movement
    public NetworkedCharacterInput(ControlState controlState)
    {
        _controlState = controlState;
    }

}