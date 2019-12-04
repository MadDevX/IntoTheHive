using System;
using System.Collections;
using System.Collections.Generic;
using GameLoop;
using UnityEngine;

public class AIKeepLineOfSightInput : UpdatableObject
{
    private ControlState _controlState;
    private Rigidbody2D _rb;
    private AITargetScanner _aiTargetScanner;
    private AIShooterScanner _aiShooterScanner;
    private AITargetInSight _aiTargetInSight;
    private AIDestinationPointScanner _aiDestinationPointScanner;

    private Transform _target;
    private MovementManager _movementManager;
    private DirectionManager _directionManager;

    public AIKeepLineOfSightInput(ControlState controlState, Rigidbody2D rigidbody, AITargetScanner aiTargetScanner, AIShooterScanner aiShooterScanner,
        AIDestinationPointScanner aiDestinationPointScanner, AITargetInSight aiTargetInSight, MovementManager movementManager, DirectionManager directionManager)
    {
        _controlState = controlState;
        _rb = rigidbody;
        _aiTargetScanner = aiTargetScanner;
        _aiShooterScanner = aiShooterScanner;
        _aiDestinationPointScanner = aiDestinationPointScanner;
        _aiTargetInSight = aiTargetInSight;
        _movementManager = movementManager;
        _directionManager = directionManager;
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

        if (_target != null && !_aiTargetInSight.TargetInSight)
        {
            _movementManager.SetMovement(_controlState, _aiDestinationPointScanner.NextPoint, _rb, _aiDestinationPointScanner.RemainingDistance);
            _directionManager.SetDirection(_controlState, _target, _rb);

            _controlState.PrimaryAction = _aiShooterScanner.ShouldShoot;
            _controlState.SecondaryAction = false;



        }
        else 
        {
            if (_target != null)
            {
                _directionManager.SetDirection(_controlState, _target, _rb);
            }
            _controlState.Horizontal = 0.0f;
            _controlState.Vertical = 0.0f;
            _controlState.PrimaryAction = _aiShooterScanner.ShouldShoot;
            _controlState.SecondaryAction = false;
        }

    }


}
