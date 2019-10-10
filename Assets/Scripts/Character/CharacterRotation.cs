using GameLoop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotation : FixedUpdatableObject
{
    private ControlState _controlState;
    private Rigidbody2D _rb;

    public CharacterRotation(ControlState controlState, Rigidbody2D rb)
    {
        _controlState = controlState;
        _rb = rb;
    }

    public override void OnFixedUpdate(float deltaTime)
    {
        _rb.rotation = Vector2.SignedAngle(Vector2.up, _controlState.Direction);
    }
}
