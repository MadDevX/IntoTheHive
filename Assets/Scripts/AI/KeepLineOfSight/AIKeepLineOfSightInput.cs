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
    private Settings _settings;
    private Vector2 _prevDirection = Vector2.zero;
    private Transform _target;

    public AIKeepLineOfSightInput(ControlState controlState, Rigidbody2D rigidbody, AITargetScanner aiTargetScanner, AIShooterScanner aiShooterScanner,
        AIDestinationPointScanner aiDestinationPointScanner, AITargetInSight aiTargetInSight, Settings settings)
    {
        _controlState = controlState;
        _rb = rigidbody;
        _aiTargetScanner = aiTargetScanner;
        _aiShooterScanner = aiShooterScanner;
        _aiDestinationPointScanner = aiDestinationPointScanner;
        _aiTargetInSight = aiTargetInSight;
        _settings = settings;
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
            Debug.Log("I should be moving now");
            //var distY = Mathf.Abs(NextPoint.y - _rb.position.y);
            //var distX = Mathf.Abs(NextPoint.x - _rb.position.x);
            var distX = 1;
            var distY = 1;
            var currentDirection = new Vector2(Mathf.Clamp(_aiDestinationPointScanner.NextPoint.x.CompareTo(_rb.position.x), -distX, distX),
                Mathf.Clamp(_aiDestinationPointScanner.NextPoint.y.CompareTo(_rb.position.y), -distY, distY));
            currentDirection = Vector2.Lerp(_prevDirection, currentDirection, _settings.LerpFactor);
          
            _controlState.Horizontal = currentDirection.x;
            _controlState.Vertical = currentDirection.y;
            

            

            //Debug.Log($"distX => {distX}, distY -> {distY}, _rb.pos -> {_rb.position}, nextPoint = {NextPoint}");
            //Debug.Log($"Horizontal -> {_controlState.Horizontal}, Vertical => {_controlState.Vertical}");
            _controlState.PrimaryAction = _aiShooterScanner.ShouldShoot;
            _controlState.SecondaryAction = false;

            _controlState.Direction = ((Vector2)_target.position - _rb.position).normalized;
            _prevDirection = currentDirection;
        }
        else 
        {
            if (_target != null)
            {
                _controlState.Direction = ((Vector2)_target.position - _rb.position).normalized;
            }
            _controlState.Horizontal = 0.0f;
            _controlState.Vertical = 0.0f;
            _controlState.PrimaryAction = _aiShooterScanner.ShouldShoot;
            _controlState.SecondaryAction = false;
        }

    }


    [Serializable]
    public class Settings
    {
        public float LerpFactor;
        //public float MinimalDistanceToPlayer;
        
    }
}
