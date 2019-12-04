using GameLoop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public struct LineVFXSpawnParameters
{
    public Vector2 from;
    public Vector2 to;
    public float ttl;
    public float width;

    public LineVFXSpawnParameters(Vector2 from, Vector2 to, float ttl, float width)
    {
        this.from = from;
        this.to = to;
        this.ttl = ttl;
        this.width = width;
    }
}

public class LineVFX : MonoUpdatableObject, IPoolable<LineVFXSpawnParameters, IMemoryPool>, IDisposable
{
    [SerializeField] private LineRenderer _renderer;
    private float _timer;
    private float _ttl;
    private float _widthMult;
    private IMemoryPool _pool;

    public void OnSpawned(LineVFXSpawnParameters parameters, IMemoryPool pool)
    {
        _pool = pool;
        _timer = 0.0f;
        _ttl = parameters.ttl;
        _widthMult = parameters.width;
        _renderer.SetPosition(0, parameters.from);
        _renderer.SetPosition(1, parameters.to);
    }

    public void Dispose()
    {
        _pool.Despawn(this);
    }

    public void OnDespawned()
    {
    }

    public override void OnUpdate(float deltaTime)
    {
        var start = _renderer.startColor;
        var end = _renderer.endColor;
        var timerFrac = (_ttl - _timer) / _ttl;
        start.a = end.a = timerFrac;
        _renderer.widthMultiplier = timerFrac * _widthMult;
        _renderer.startColor = start;
        _renderer.endColor = end;
        _timer += deltaTime;

        if(_timer >= _ttl)
        {
            Dispose();
        }
    }

    public class Factory : PlaceholderFactory<LineVFXSpawnParameters, LineVFX>
    {
    }
}
