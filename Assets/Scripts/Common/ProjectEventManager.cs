using System;
using UnityEngine;
using Zenject;

/// <summary>
/// This class functions as an inter-scene communication. 
/// </summary>
public class ProjectEventManager
{
    public event Action GameInitializedHost;

    public void FireGameInitializedHost()
    {
        GameInitializedHost?.Invoke();
    }
}