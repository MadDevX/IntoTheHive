using GameLoop;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public struct FloatingTextSpawnParameters
{
    public Vector2 position;
    public int damage;
    public bool isPlayer;
    public FloatingTextSpawnParameters(Vector2 position, int damage, bool isPlayer = false)
    {
        this.position = position;
        this.damage = damage;
        this.isPlayer = isPlayer;
    }
}

public class FloatingText : MonoUpdatableObject, IPoolable<FloatingTextSpawnParameters, IMemoryPool>, IDisposable
{
    [SerializeField] private TextMeshProUGUI _text;

    private Camera _camera;
    private IMemoryPool _pool;
    private Settings _settings;

    private Vector3 _spawnPosition;
    private float _timer;


    [Inject]
    public void Construct(Camera camera, Settings settings)
    {
        _camera = camera;
        _settings = settings;
    }

    public void Dispose()
    {
        _pool?.Despawn(this);
    }

    public void OnDespawned()
    {
        _pool = null;
    }

    public void OnSpawned(FloatingTextSpawnParameters parameters, IMemoryPool pool)
    {
        _pool = pool;
        _text.color = parameters.damage > 0 ? (parameters.isPlayer ? _settings.playerDamageColor : _settings.damageColor) : _settings.healColor;
        _text.text = Mathf.Abs(parameters.damage).ToString();
        _spawnPosition = parameters.position;
        transform.position = CalculatePosition(GetPosition());
        _timer = 0.0f;
    }

    public override void OnUpdate(float deltaTime)
    {
        if (_timer >= _settings.ttl)
        {
            Dispose();
            return;
        }
        transform.position = CalculatePosition(GetPosition() + Vector3.up * _settings.floatSpeed * _timer);
        _text.alpha = 1.0f - (_timer / _settings.ttl);
        _timer += deltaTime;
    }

    private Vector2 CalculatePosition(Vector2 position)
    {
        return _camera.WorldToScreenPoint(position);
    }

    private Vector3 GetPosition()
    {
        return _spawnPosition + _settings.offset;
    }


    public class Factory : PlaceholderFactory<FloatingTextSpawnParameters, FloatingText>
    {
    }

    [System.Serializable]
    public class Settings
    {
        public float floatSpeed;
        public float ttl;
        public Color damageColor;
        public Color healColor;
        public Color playerDamageColor;
        public Vector3 offset;
    }
}
