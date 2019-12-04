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
    private AIDestinationPointScanner _aiDestinationPointScanner;
    private DirectionManager _directionManager;
    private MovementManager _movementManager;


    private Transform _target;
    public AIFollowInput(ControlState controlState, Rigidbody2D rigidbody, AITargetScanner aiTargetScanner, AIShooterScanner aiShooterScanner,
        AIDestinationPointScanner aiDestinationPointScanner, DirectionManager directionManager, MovementManager movementManager)
    {
        _controlState = controlState;
        _rb = rigidbody;
        _aiTargetScanner = aiTargetScanner;
        _aiShooterScanner = aiShooterScanner;
        _aiDestinationPointScanner = aiDestinationPointScanner;
        _directionManager = directionManager;
        _movementManager = movementManager;
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
           
            _movementManager.SetMovement(_controlState, _aiDestinationPointScanner.NextPoint, _rb, _aiDestinationPointScanner.RemainingDistance);
            _directionManager.SetDirection(_controlState, _target, _rb);

            _controlState.PrimaryAction = _aiShooterScanner.ShouldShoot;
            _controlState.SecondaryAction = false;


        }
        else
        {
            _controlState.Horizontal = 0.0f;
            _controlState.Vertical = 0.0f;
            _controlState.PrimaryAction = _aiShooterScanner.ShouldShoot;
            _controlState.SecondaryAction = false;
        }
        
    }


    

}
