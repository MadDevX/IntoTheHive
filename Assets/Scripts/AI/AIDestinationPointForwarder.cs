using System;
using System.Collections;
using System.Collections.Generic;
using GameLoop;
using Pathfinding;
using UnityEngine;
using Zenject;

public class AIDestinationPointForwarder : UpdatableObject
{
    private AIPath _aiPath;
    private AIInput _aiInput;

    public AIDestinationPointForwarder(AIPath aiPath, AIInput aiInput)
    {
        _aiPath = aiPath;
        _aiInput = aiInput;
    }

    public override void OnUpdate(float deltaTime)
    {
        _aiInput.NextPoint = _aiPath.steeringTarget;
    }
}
