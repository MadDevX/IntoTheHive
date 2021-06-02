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
    [SerializeField] private SpriteRenderer _renderer;

    private float _timer;
    private float _ttl;
    private float _widthMult;
    private IMemoryPool _pool;
    private Color _color = Color.white;

    public void OnSpawned(LineVFXSpawnParameters parameters, IMemoryPool pool)
    {
        _pool = pool;
        _timer = 0.0f;
        _ttl = parameters.ttl;
        _widthMult = parameters.width;
        transform.position = parameters.from;
        var dir = parameters.to - parameters.from;
        var dirMag = dir.magnitude;
        _renderer.size = new Vector2(_renderer.size.x, dirMag);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, dir.Rotation());
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
        var timerFrac = (_ttl - _timer) / _ttl;
        transform.localScale = new Vector3((timerFrac * 0.75f + 0.25f) * _widthMult, transform.localScale.y, transform.localScale.z);
        _color.a = timerFrac;
        _renderer.color = _color;
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
