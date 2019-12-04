using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionManager 
{
    private Settings _settings;

    public DirectionManager(Settings settings)
    {
        _settings = settings;
    }
    public void SetDirection(ControlState controlState, Transform target, Rigidbody2D character)
    {
        controlState.Direction = Vector2.Lerp(controlState.Direction, ((Vector2)target.position - character.position).normalized, _settings.RotationLerp);
    }

    [Serializable]
    public class Settings
    {
        public float RotationLerp;
    }

}
