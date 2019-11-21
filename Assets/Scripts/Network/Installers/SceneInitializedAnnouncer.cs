using System;
using UnityEngine;

public class SceneInitializedAnnouncer: MonoBehaviour
{
    public event Action SceneInitialized;
        
    private void Start()
    {
        SceneInitialized?.Invoke();
        Debug.Log("Scene Initialized");
    }
}