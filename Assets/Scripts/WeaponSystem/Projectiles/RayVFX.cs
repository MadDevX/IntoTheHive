using GameLoop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RayVFX// : MonoUpdatableObject, IPoolable<ProjectileSpawnParameters, IMemoryPool>, IDisposable, IProjectile
{
    [SerializeField] private LineRenderer _lineRenderer;

    public float TTL { get; set; }


}
