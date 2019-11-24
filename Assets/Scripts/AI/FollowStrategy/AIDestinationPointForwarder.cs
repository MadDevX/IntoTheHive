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
    private AIFollowInput _aiFollowInput;

    public AIDestinationPointForwarder(AIPath aiPath, AIFollowInput aiFollowInput)
    {
        _aiPath = aiPath;
        _aiFollowInput = aiFollowInput;
    }

    public override void OnUpdate(float deltaTime)
    {
        //_aiPath.MovementUpdate(deltaTime, out var nextPosition, out _);
        //if(!(_aiFollowInput.NextPoint == (Vector2) nextPosition))
        //    Debug.Log($"Previous point -> {_aiFollowInput.NextPoint}, next point -> {nextPosition}, currently at {_aiPath.position}");
        //_aiFollowInput.NextPoint = nextPosition;
        //Debug.Log($"Previous point -> {_aiFollowInput.NextPoint}, next point -> {_aiPath.steeringTarget}, currently at {_aiPath.position}");
        _aiFollowInput.NextPoint = _aiPath.steeringTarget;
    }
}
