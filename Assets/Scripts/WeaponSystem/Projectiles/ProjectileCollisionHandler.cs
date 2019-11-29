using Relays;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectileCollisionHandler : IInitializable, IDisposable
{
    public event Action<IProjectile, Collider2D> OnCollisionEnter;

    private IRelay _relay;
    private IProjectile _facade;

    public ProjectileCollisionHandler(IRelay relay, IProjectile facade)
    {
        _relay = relay;
        _facade = facade;
    }

    public void Initialize()
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
        OnCollisionEnter?.Invoke(_facade, obj);
    }

    private void OnColEnterHandler(Collision2D obj)
    {
        OnCollisionEnter?.Invoke(_facade, obj.collider);
    }
}
