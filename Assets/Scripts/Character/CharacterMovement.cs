using GameLoop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : FixedUpdatableObject
{
    private Rigidbody2D _rb;
    private ControlState _controlState;
    private Settings _settings;
    private Vector2 _movementVersor;

    public Vector2 MovementVersor
    {
        get
        {
            return _movementVersor;
        }

        private set
        {
            _movementVersor = Vector2.ClampMagnitude(value, 1.0f);
        }
    }
    
    public CharacterMovement(Rigidbody2D rb, ControlState controlState, Settings settings)
    {
        _rb = rb;
        _controlState = controlState;
        _settings = settings;
    }

    public override void Initialize()
    {
        base.Initialize();
        _controlState.Position = _rb.position;
    }

    public override void OnFixedUpdate(float deltaTime)
    {
        UpdateMovementVersor(deltaTime);
        CorrectPosition(deltaTime);
        Move(deltaTime);
    }

    private void CorrectPosition(float deltaTime)
    {
        var positionDifference = (_rb.position-_controlState.Position).sqrMagnitude;

        if (positionDifference >= _settings.positionEps * _settings.positionEps)
        {
            _rb.MovePosition(Vector2.Lerp(_rb.position, _controlState.Position,_settings.correctionLerpFactor));
        }
        
    }

    private void UpdateMovementVersor(float deltaTime)
    {
        MovementVersor = new Vector2(_controlState.Horizontal, _controlState.Vertical);
    }

    private void Move(float deltaTime)
    {
        _rb.velocity = MovementVersor * CalculateSpeedBonus();
    }

    private float CalculateSpeedBonus()
    {
        return _controlState.SecondaryAction ? _settings.speedMult * _settings.baseSpeed : _settings.baseSpeed; 
    }

    [System.Serializable]
    public class Settings
    {
        public float positionEps;
        public float baseSpeed;
        public float speedMult;
        public float correctionLerpFactor;
    }

}
