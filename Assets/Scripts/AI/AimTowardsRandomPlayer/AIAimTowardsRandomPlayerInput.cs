using System.Collections;
using System.Collections.Generic;
using GameLoop;
using UnityEngine;

public class AIAimTowardsRandomPlayerInput : UpdatableObject
{
    private ControlState _controlState;
    private Rigidbody2D _rb;
    private AITargetScanner _aiTargetScanner;
    private AIShooterScanner _aiShooterScanner;
    private Transform _target;
    private DirectionManager _directionManager;


    public AIAimTowardsRandomPlayerInput(ControlState controlState, Rigidbody2D rigidbody, AITargetScanner aiTargetScanner,
        AIShooterScanner aiShooterScanner, DirectionManager directionManager)
    {
        _controlState = controlState;
        _rb = rigidbody;
        _aiTargetScanner = aiTargetScanner;
        _aiShooterScanner = aiShooterScanner;
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
        _controlState.Horizontal = 0.0f;
        _controlState.Vertical = 0.0f;
        _controlState.SecondaryAction = false;

        if (_target != null)
        {
            _controlState.PrimaryAction = _aiShooterScanner.ShouldShoot;
            _directionManager.SetDirection(_controlState, _target, _rb);
        }


    }
}