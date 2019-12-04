using System;
using System.Collections;
using System.Collections.Generic;
using GameLoop;
using UnityEngine;

public class AIShootOnTimer : FixedUpdatableObject
{
    private Settings _settings;
    private float _timer = 0.0f;
    public bool ShouldShoot { get; private set; }

    public AIShootOnTimer(Settings settings)
    {
        _settings = settings;
    }

    public override void OnFixedUpdate(float deltaTime)
    {
        if (_timer >= _settings.shootDelay)
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

    [Serializable]
    public class Settings
    {
        public float shootDelay;
    }
}
