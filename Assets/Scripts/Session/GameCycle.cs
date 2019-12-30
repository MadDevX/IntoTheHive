using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCycle : IGameCycle, IGameCycleController
{
    public event Action OnGameStarted;
    public event Action OnGameEnded;

    public void RaiseOnGameStarted()
    {
        OnGameStarted?.Invoke();
    }

    public void RaiseOnGameEnded()
    {
        OnGameEnded?.Invoke();
    }
}
