using GameLoop;
using System;
using UnityEngine;

public class SceneInitializedAnnouncer: MonoUpdatableObject
{
    public event Action SceneInitialized;

    private void Start()
    {
        SceneInitialized?.Invoke();
        Debug.Log("Scene Initialized");
    }

    public override void OnUpdate(float deltaTime)
    {
        UnsubscribeLoop();
    }
}