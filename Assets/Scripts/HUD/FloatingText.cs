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
    private Vector3 _worldPosition;
    private float _timer;

    private Vector3 _velocity;

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
        _velocity = CalculateInitialVelocity();
        _worldPosition = GetPosition();
        transform.position = CalculateCanvasPosition(_worldPosition);
        _timer = 0.0f;
    }

    public override void OnUpdate(float deltaTime)
    {
        if (_timer >= _settings.ttl)
        {
            Dispose();
            return;
        }
        UpdateVelocity(deltaTime);
        _worldPosition += _velocity * deltaTime;
        transform.position = CalculateCanvasPosition(_worldPosition);
        _text.alpha = 1.0f - (_timer / _settings.ttl);
        _timer += deltaTime;
    }

    private Vector2 CalculateCanvasPosition(Vector2 position)
    {
        return _camera.WorldToScreenPoint(position);
    }

    private Vector3 CalculateInitialVelocity()
    {
        return Vector2.up.Rotate(UnityEngine.Random.Range(-_settings.angleDiff, _settings.angleDiff)) * _settings.floatSpeed;
    }

    private void UpdateVelocity(float deltaTime)
    {
        _velocity += Vector3.down * _settings.gravity * deltaTime;
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
        public float angleDiff;
        public float gravity;
        public Color damageColor;
        public Color healColor;
        public Color playerDamageColor;
        public Vector3 offset;
    }
}
