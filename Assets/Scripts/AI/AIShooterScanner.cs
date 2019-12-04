using System;
using System.Collections;
using System.Collections.Generic;
using GameLoop;
using UnityEngine;
using Zenject;

public class AIShooterScanner : FixedUpdatableObject
{
    private AITargetScanner _aiTargetScanner;
    private Settings _settings;
    private float _timer = 0.0f;
    private bool _targetAcquired;
    public bool ShouldShoot { get; private set; }
    public AIShooterScanner(AITargetScanner aiTargetScanner, Settings settings)
    {
        _aiTargetScanner = aiTargetScanner;
        _settings = settings;
    }

    public override void OnFixedUpdate(float deltaTime)
    {
        if (_targetAcquired && _timer >= _settings.shootDelay)
        {
            ShouldShoot = true;
            _timer = 0.0f;

        }
        else
        {
            ShouldShoot = false;
        }
        _timer += deltaTime;
    }
    public override void Initialize()
    {
        base.Initialize();
        _aiTargetScanner.OnTargetChanged += OnTargetChanged;
    }

    public override void Dispose()
    {
        base.Dispose();
        _aiTargetScanner.OnTargetChanged -= OnTargetChanged;
    }

    private void OnTargetChanged(Transform transform)
    {
        _targetAcquired = transform != null;
        //_timer = 0.0f;
    }

    [Serializable]
    public class Settings
    {
        public float shootDelay;
    }
}

