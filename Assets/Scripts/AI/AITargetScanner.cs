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

    public AITargetScanner(Rigidbody2D rb, Settings settings)
    {
        _rb = rb;
        _settings = settings;
    }

    public override void OnFixedUpdate(float deltaTime)
    {
        if (_timer >= _settings.scanDelay)
        {
            var hits = Physics2D.OverlapCircleAll(_rb.position, _settings.scanRadius);
            _timer = 0.0f;

            UpdateTarget(hits);
        }
        _timer += deltaTime;
    }

    private void UpdateTarget(Collider2D[] hits)
    {
        for(int i = 0; i < hits.Length; i++)
        {
            var player = hits[i].GetComponent<PlayerInstaller>();
            if(player != null)
            {
                Target = player.transform;
                Debug.Log($"Found target: {Target.name}");
                return;
            }
        }

        Target = null;
    }

    [System.Serializable]
    public class Settings
    {
        public float scanDelay;
        public float scanRadius;
    }
}
