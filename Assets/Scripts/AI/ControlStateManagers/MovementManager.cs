using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager
{
    private Settings _settings;

    public MovementManager(Settings settings)
    {
        _settings = settings;
    }
    public void SetMovement(ControlState controlState, Vector2 nextPoint, Rigidbody2D character, float remainingDistance)
    {
        //var distY = Mathf.Abs(nextPoint.y - character.position.y);
        //var distX = Mathf.Abs(nextPoint.x - character.position.x);
        var distX = 1;
        var distY = 1;
        var currentDirection = new Vector2(Mathf.Clamp(nextPoint.x.CompareTo(character.position.x), -distX, distX),
            Mathf.Clamp(nextPoint.y.CompareTo(character.position.y), -distY, distY));
        currentDirection = Vector2.Lerp(new Vector2(controlState.Horizontal, controlState.Vertical), currentDirection, _settings.LerpFactor);



        if (remainingDistance <= _settings.DistanceToPlayerToStop)
        {
            controlState.Horizontal = 0.0f;
            controlState.Vertical = 0.0f;
        }
        else
        {
            var velocityMultiplier = Mathf.Clamp((remainingDistance - _settings.DistanceToPlayerToStop) /
                                                 (_settings.DistanceToPlayerToStartSlowing -
                                                  _settings.DistanceToPlayerToStop), 0.05f, 1.0f);

            controlState.Horizontal = currentDirection.x * velocityMultiplier;
            controlState.Vertical = currentDirection.y * velocityMultiplier;
        }

        


        //Debug.Log($"distX => {distX}, distY -> {distY}, _rb.pos -> {_rb.position}, nextPoint = {NextPoint}");
        //Debug.Log($"Horizontal -> {_controlState.Horizontal}, Vertical => {_controlState.Vertical}");

    }

    [Serializable]
    public class Settings
    {
        public float LerpFactor;
        public float DistanceToPlayerToStop;
        public float DistanceToPlayerToStartSlowing;
    }
}
