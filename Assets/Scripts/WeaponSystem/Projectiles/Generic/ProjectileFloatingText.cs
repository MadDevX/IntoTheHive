using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectileFloatingText : IDisposable
{
    private ProjectileDamage _damage;
    private FloatingText.Factory _floatingTextFactory;
    private readonly PlayerRegistry _registry;

    public ProjectileFloatingText(
        ProjectileDamage damage,
        [Inject(Id = Identifiers.HUD)] FloatingText.Factory floatingTextFactory,
        PlayerRegistry registry)
    {
        _damage = damage;
        _floatingTextFactory = floatingTextFactory;
        _registry = registry;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _damage.OnDamageDealt += OnDamageDealt;
    }

    public void Dispose()
    {
        _damage.OnDamageDealt -= OnDamageDealt;
    }

    private void OnDamageDealt(DamageDealtParameters obj)
    {
        if (obj.hitParameters.transform != _registry.Player?.transform)
        {
            _floatingTextFactory.Create(new FloatingTextSpawnParameters(obj.hitParameters.transform.position, Mathf.RoundToInt(obj.damage)));
        }
    }
}
