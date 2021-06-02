using System;
using System.Collections;
using System.Collections.Generic;
using GameLoop;
using Pathfinding;
using UnityEngine;
using Zenject;

public class AIDestinationPointScanner : UpdatableObject
{
    private AIPath _aiPath;

    public Vector2 NextPoint {  get; private set; }
    public float RemainingDistance { get; private set; }

    public AIDestinationPointScanner(AIPath aiPath)
    {
        _aiPath = aiPath;
    }

    public override void OnUpdate(float deltaTime)
    {
        //_aiPath.MovementUpdate(deltaTime, out var nextPosition, out _);
        //if(!(_aiFollowInput.NextPoint == (Vector2) nextPosition))
        //    Debug.Log($"Previous point -> {_aiFollowInput.NextPoint}, next point -> {nextPosition}, currently at {_aiPath.position}");
        //_aiFollowInput.NextPoint = nextPosition;
        //Debug.Log($"Previous point -> {_aiFollowInput.NextPoint}, next point -> {_aiPath.steeringTarget}, currently at {_aiPath.position}");
        
        NextPoint = _aiPath.steeringTarget;
        RemainingDistance = _aiPath.remainingDistance;
    }
}
