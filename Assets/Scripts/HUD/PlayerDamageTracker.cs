using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerDamageTracker : IInitializable, IDisposable
{
    private PlayerRegistry _registry;
    private FloatingText.Factory _floatingTextFactory;

    public PlayerDamageTracker(PlayerRegistry registry, [Inject(Id = Identifiers.HUD)] FloatingText.Factory floatingTextFactory)
    {
        _registry = registry;
        _floatingTextFactory = floatingTextFactory;
    }

    public void Initialize()
    {
        _registry.OnPlayerSet += OnSet;
        _registry.OnPlayerUnset += OnUnset;
    }

    public void Dispose()
    {
        _registry.OnPlayerSet -= OnSet;
        _registry.OnPlayerUnset -= OnUnset;
    }

    private void OnSet(CharacterFacade obj)
    {
        obj.OnDamageTaken += OnDamageTaken;
    }

    private void OnUnset(CharacterFacade obj)
    {
        obj.OnDamageTaken -= OnDamageTaken;
    }

    private void OnDamageTaken(DamageTakenArgs obj)
    {
        _floatingTextFactory.Create(new FloatingTextSpawnParameters(_registry.Player.Position, Mathf.RoundToInt(obj.damage), true));
    }



}
