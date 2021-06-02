using System.Collections;
using System.Collections.Generic;
using GameLoop;
using UnityEngine;

public class AIAimOnlyOneDirectionInput : UpdatableObject
{
    private ControlState _controlState;
    private AIShootOnTimer _aiShootOnTimer;

    private float _timer;
    public AIAimOnlyOneDirectionInput(ControlState controlState, AIShootOnTimer aiShootOnTimer)
    {
        _controlState = controlState;
        _aiShootOnTimer = aiShootOnTimer;
    }

    

    public override void OnUpdate(float deltaTime)
    {
        _controlState.Horizontal = 0.0f;
        _controlState.Vertical = 0.0f;
        _controlState.SecondaryAction = false;
        _controlState.PrimaryAction = _aiShootOnTimer.ShouldShoot;
        _controlState.Direction = Vector2.up;
    }
}
