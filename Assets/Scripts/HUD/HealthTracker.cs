using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;
using System;

public class HealthTracker : IDisposable
{
    private TextMeshProUGUI _text;
    private Settings _settings;
    private PlayerRegistry _registry;

    public HealthTracker([Inject(Id = Identifiers.Health)] TextMeshProUGUI text, Settings settings, PlayerRegistry registry)
    {
        _text = text;
        _settings = settings;
        _registry = registry;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _registry.OnPlayerSet += OnPlayerSet;
        _registry.OnPlayerUnset += OnPlayerUnset;
    }

    public void Dispose()
    {
        _registry.OnPlayerSet -= OnPlayerSet;
        _registry.OnPlayerUnset -= OnPlayerUnset;
    }

    private void OnPlayerSet(CharacterFacade obj)
    {
        obj.OnDamageTaken += SetText;
        SetText(0.0f);
    }

    private void OnPlayerUnset(CharacterFacade obj)
    {
        obj.OnDamageTaken -= SetText;
    }

    private void SetText(float dmg)
    {
        _text.text = _settings.header + _registry.Player.Health;
    }


    [System.Serializable]
    public class Settings
    {
        public string header;
    }
}
