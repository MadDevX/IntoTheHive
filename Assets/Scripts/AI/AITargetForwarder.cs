using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Zenject;

public class AITargetForwarder : IInitializable, IDisposable
{
    private AIDestinationSetter _aiDestinationSetter;
    private AITargetScanner _aiTargetScanner;

    public AITargetForwarder(AIDestinationSetter aiDestinationSetter, AITargetScanner aiTargetScanner)
    {
        _aiDestinationSetter = aiDestinationSetter;
        _aiTargetScanner = aiTargetScanner;
    }
    public void Initialize()
    {
        _aiTargetScanner.OnTargetChanged += OnTargetChanged;
    }

    public void Dispose()
    {
        _aiTargetScanner.OnTargetChanged -= OnTargetChanged;
    }

    private void OnTargetChanged(Transform transform)
    {
        _aiDestinationSetter.target = transform;
    }
}
