﻿using System;
using UnityEngine;
using Zenject;

/// <summary>
/// This class functions as an inter-scene communication. 
/// </summary>
public class ProjectEventManager
{
    public event Action GameInitializedHost;
    public event Action LobbyInitializedHost;
    public event Action GameEndedMenuInitializedHost;
    public void FireGameInitializedHost()
    {
        GameInitializedHost?.Invoke();
    }

    public void FireLobbyInitializedHost()
    {
        LobbyInitializedHost?.Invoke();
    }

    public void FireGameEndedMenuInitializedHost()
    {
        GameEndedMenuInitializedHost?.Invoke();
    }
}