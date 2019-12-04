using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RayProjectileVFX : IDisposable
{
    private RayProjectileRaycaster _raycaster;
    private LineVFX.Factory _factory;
    private Settings _settings;

    public RayProjectileVFX(RayProjectileRaycaster raycaster, [Inject(Id = Identifiers.Ray)] LineVFX.Factory factory, Settings settings)
    {
        _raycaster = raycaster;
        _factory = factory;
        _settings = settings;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _raycaster.OnRayExecuted += OnRayExecuted;
    }

    public void Dispose()
    {
        _raycaster.OnRayExecuted -= OnRayExecuted;
    }

    private void OnRayExecuted(Vector2 from, Vector2 to)
    {
        _factory.Create(new LineVFXSpawnParameters(from, to, _settings.vfxDuration, _settings.widthMult));
    }

    [System.Serializable]
    public class Settings
    {
        public float vfxDuration;
        public float widthMult;
    }
}
