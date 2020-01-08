using Relays;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RigidProjectileCollisionHandler : IDisposable, IProjectileCollision
{
    public event Action<Collider2D> OnCollisionEnter;
    public event Action AfterCollisionEnter;

    private Collider2D _collider;
    private IRelay _relay;
    private ProjectilePhasePipeline _pipeline;

    public bool IsPiercing { get => _collider.isTrigger; set => _collider.isTrigger = value; }

    public RigidProjectileCollisionHandler(IRelay relay, Collider2D collider, ProjectilePhasePipeline pipeline)
    {
        _relay = relay;
        _pipeline = pipeline;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _relay.OnCollision2DEnterEvt += OnColEnterHandler;
        _relay.OnTrigger2DEnterEvt += OnTriggerEnterHandler;
    }

    public void Dispose()
    {
        _relay.OnCollision2DEnterEvt -= OnColEnterHandler;
        _relay.OnTrigger2DEnterEvt -= OnTriggerEnterHandler;
    }

    private void OnTriggerEnterHandler(Collider2D obj)
    {
        if (obj.isTrigger == false && _pipeline.State != ProjectilePhases.Destroyed) //TODO: check object layers, this is a hack
        {
            OnCollisionEnter?.Invoke(obj);
            AfterCollisionEnter?.Invoke();
        }
    }

    private void OnColEnterHandler(Collision2D obj)
    {
        if (obj.collider.isTrigger == false && _pipeline.State != ProjectilePhases.Destroyed)
        {
            OnCollisionEnter?.Invoke(obj.collider);
            AfterCollisionEnter?.Invoke();
        }
    }
}
