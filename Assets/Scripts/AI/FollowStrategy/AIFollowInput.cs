using System;
using GameLoop;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AIFollowInput : UpdatableObject
{
    private ControlState _controlState;
    private Rigidbody2D _rb;
    private AITargetScanner _aiTargetScanner;
    private AIShooterScanner _aiShooterScanner;
    public Vector2 NextPoint { get; set; }
    private Transform _target;
    public AIFollowInput(ControlState controlState, Rigidbody2D rigidbody, AITargetScanner aiTargetScanner, AIShooterScanner aiShooterScanner)
    {
        _controlState = controlState;
        _rb = rigidbody;
        _aiTargetScanner = aiTargetScanner;
        _aiShooterScanner = aiShooterScanner;
    }

    public override void Initialize()
    {
        base.Initialize();
        _aiTargetScanner.OnTargetChanged += AiTargetScannerOnTargetChanged;
    }

    private void AiTargetScannerOnTargetChanged(Transform obj)
    {
        _target = obj;
    }

    public override void Dispose()
    {
        base.Dispose();
        _aiTargetScanner.OnTargetChanged -= AiTargetScannerOnTargetChanged;
    }

    public override void OnUpdate(float deltaTime)
    {
        if (_target != null)
        {
            var distY = Mathf.Abs(_target.position.y - _rb.position.y);
            var distX = Mathf.Abs(_target.position.x - _rb.position.x);
            _controlState.Horizontal = Mathf.Clamp(NextPoint.x.CompareTo(_rb.position.x), -distX, distX);
            _controlState.Vertical = Mathf.Clamp(NextPoint.y.CompareTo(_rb.position.y), -distY, distY);
            _controlState.PrimaryAction = _aiShooterScanner.ShouldShoot;
            _controlState.SecondaryAction = false;

            _controlState.Direction = ((Vector2) _target.position - _rb.position).normalized;
        }
        else
        {
            _controlState.Horizontal = 0.0f;
            _controlState.Vertical = 0.0f;
            _controlState.PrimaryAction = false;
            _controlState.SecondaryAction = false;
        }
    }
}
