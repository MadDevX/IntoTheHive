using GameLoop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AITargetScanner : FixedUpdatableObject
{
    private Rigidbody2D _rb;
    private Settings _settings;
    private ITargetUpdatable _targetUpdatable;

    public Transform Target
    {
        get => _target;
        set
        {
            if (value != _target)
            {
                _target = value;
                OnTargetChanged?.Invoke(value);
            }
        }
    }
    public event Action<Transform> OnTargetChanged;
    private Transform _target;

    private float _timer = 0.0f;

    public AITargetScanner(Rigidbody2D rb, Settings settings, ITargetUpdatable targetUpdatable)
    {
        _rb = rb;
        _settings = settings;
        _targetUpdatable = targetUpdatable;
    }

    public override void OnFixedUpdate(float deltaTime)
    {
        if (_timer >= _settings.scanDelay)
        {
            var hits = Physics2D.OverlapCircleAll(_rb.position, _settings.scanRadius, ~Layers.Environment.ToMask());
            Array.Sort(hits, (x1, x2) => ((Vector2)x1.transform.position - _rb.position).sqrMagnitude
                .CompareTo(((Vector2)x2.transform.position - _rb.position).sqrMagnitude));
           

            _timer = 0.0f;
            Target = _targetUpdatable.GetTarget(hits);
        }
        _timer += deltaTime;
    }
    

    [System.Serializable]
    public class Settings
    {
        public float scanDelay;
        public float scanRadius;
    }
}



