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
    private CharacterInfo _info;
    private Vector2 _movementVersor;

    /// <summary>
    /// This property represents movement direction and speed. 
    /// It automatically clamps its magnitude to avoid exceeding 
    /// speed limits by moving diagonally.
    /// </summary>
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
    
    public CharacterMovement(Rigidbody2D rb, ControlState controlState, Settings settings, CharacterInfo info)
    {
        _rb = rb;
        _controlState = controlState;
        _settings = settings;
        _info = info;
    }

    public override void Initialize()
    {
        base.Initialize();
        _controlState.Position = _rb.position;
    }

    public override void OnFixedUpdate(float deltaTime)
    {
        UpdateMovementVersor(deltaTime);
        if(_info.IsLocal == false) CorrectPosition(deltaTime);
        Move(deltaTime);
        ResetAngularVelocity(); //used because other logic governs object rotation and angular velocity should not affect character rotation
    }

    private void CorrectPosition(float deltaTime)
    {
        var positionDifference = (_rb.position-_controlState.Position).sqrMagnitude;

        if (positionDifference >= _settings.teleportEps * _settings.teleportEps)
        {
            _rb.position = _controlState.Position;
        }
        else
        {
            if (positionDifference >= _settings.positionEps * _settings.positionEps)
            {
                _rb.MovePosition(Vector2.Lerp(_rb.position, _controlState.Position,_settings.correctionLerpFactor));
            }       
        }
        
    }

    /// <summary>
    /// Updates <paramref name="basePosition"/> based on current ControlState and <paramref name="deltaTime"/>
    /// </summary>
    /// <param name="basePosition">Position to translate</param>
    /// <param name="deltaTime">How much time needs to be simulated</param>
    /// <returns></returns>
    public Vector2 UpdatePosition(Vector2 basePosition, float deltaTime)
    {
        return basePosition + MovementVersor * CalculateSpeedBonus() * deltaTime;
    }

    private void UpdateMovementVersor(float deltaTime)
    {
        MovementVersor = new Vector2(_controlState.Horizontal, _controlState.Vertical);
    }

    private void Move(float deltaTime)
    {
        _rb.velocity = MovementVersor * CalculateSpeedBonus();
    }

    private void ResetAngularVelocity()
    {
        _rb.angularVelocity = 0.0f;
    }

    private float CalculateSpeedBonus()
    {
        return _controlState.SecondaryAction ? _settings.speedMult * _settings.baseSpeed : _settings.baseSpeed; 
    }

    [System.Serializable]
    public class Settings
    {
        public float positionEps;
        public float teleportEps;
        public float baseSpeed;
        public float speedMult;
        public float correctionLerpFactor;
    }

}
