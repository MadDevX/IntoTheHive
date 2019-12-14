﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: you already have OnParticleInitialized event. why not create separate class that controls pipeline during this event?
public class RayProjectileRaycaster: IDisposable
{
    public event Action<Vector2, Vector2> OnRayExecuted; //TODO: rework this, should also contain info about modules

    private ProjectileInitializer _initializer;
    private IProjectilePosition _position;
    private IProjectileVelocity _velocity;
    private IProjectileCollisionHandler _colHandler;
    private Settings _settings;
    private int _layerMask;

    public RayProjectileRaycaster(
        ProjectileInitializer initializer, 
        IProjectilePosition position, 
        IProjectileVelocity velocity,
        IProjectileCollisionHandler colHandler,
        Settings settings)
    {
        _initializer = initializer;
        _position = position;
        _velocity = velocity;
        _colHandler = colHandler;
        _settings = settings;
        _layerMask = Layers.Interactable.ToMask() + Layers.Environment.ToMask();
        PreInitialize();
    }

    private void PreInitialize()
    {
        _initializer.OnProjectileInitialized += CastProjectile;
    }

    public void Dispose()
    {
        _initializer.OnProjectileInitialized -= CastProjectile;
    }

    private void CastProjectile()
    {
        var hit = Physics2D.RaycastAll(_position.Position, _velocity.Velocity, _velocity.Velocity.magnitude * _settings.baseRange, _layerMask);
        var startPos = _position.Position;
        Vector2 lastPos = _position.Position + _velocity.Velocity * _settings.baseRange;
        for(int i = 0; i < hit.Length; i++)
        {
            //TODO: check bullet state to avoid excess collisions
            _velocity.Velocity = Vector2.Reflect(_velocity.Velocity, hit[i].normal);
            _position.Position = hit[i].point;
            _colHandler.HandleCollision(hit[i].collider);
            lastPos = hit[i].point;
        }
        OnRayExecuted?.Invoke(startPos, lastPos);
    }

    [System.Serializable]
    public class Settings
    {
        public float baseRange;
    }
}