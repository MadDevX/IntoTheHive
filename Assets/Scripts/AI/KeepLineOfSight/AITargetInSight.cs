using System;
using System.Collections;
using System.Collections.Generic;
using GameLoop;
using UnityEngine;
using Zenject;

public class AITargetInSight : FixedUpdatableObject
{
    private AITargetScanner _aiTargetScanner;
    private Transform _transform;
    private Rigidbody2D _rb;
    private Settings _settings;

    //Determines if given target is in sight, given infinite sight
    public bool TargetInSight { get; private set; }

    public AITargetInSight(AITargetScanner aiTargetScanner, Rigidbody2D rigidbody, Settings settings)
    {
        _aiTargetScanner = aiTargetScanner;
        _rb = rigidbody;
        _settings = settings;
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
        _transform = transform;
    }

    public override void OnFixedUpdate(float deltaTime)
    {
        if (_transform != null)
        {
            var diff = (Vector2)_transform.position - _rb.position;
            var hit = Physics2D.CircleCast(_rb.position, _settings.castRadius, diff, diff.magnitude, Layers.Environment.ToMask());

            //var hit = Physics2D.Linecast(_rb.position, _transform.position, Layers.Environment.ToMask());
            // If collider is not null, that means there is a obstacle between his target
            TargetInSight = hit.collider == null;
        }
    }

    [Serializable]
    public class Settings
    {
        public float castRadius;
    }
}
