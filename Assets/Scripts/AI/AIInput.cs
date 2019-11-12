using System;
using GameLoop;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AIInput : UpdatableObject
{
    private ControlState _controlState;
    private Rigidbody2D _rb;
    private AITargetScanner _aiTargetScanner;

    public Vector2 NextPoint { get; set; }
    private Transform _target;
    public AIInput(ControlState controlState, Rigidbody2D rigidbody, AITargetScanner aiTargetScanner)
    {
        _controlState = controlState;
        _rb = rigidbody;
        _aiTargetScanner = aiTargetScanner;
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
            _controlState.PrimaryAction = false;
            _controlState.SecondaryAction = false;

            _controlState.Direction = ((Vector2) _target.position - _rb.position).normalized;
        }
    }
}
