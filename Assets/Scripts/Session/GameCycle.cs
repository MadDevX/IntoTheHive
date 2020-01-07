using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCycle : IGameCycle, IGameCycleController
{
    public event Action OnGameStarted;
    public event Action OnGameEnded;
    public event Action OnGameWon;

    public void RaiseOnGameStarted()
    {
        OnGameStarted?.Invoke();
    }

    public void RaiseOnGameEnded()
    {
        OnGameEnded?.Invoke();
    }

    public void RaiseOnGameWon()
    {
        OnGameWon?.Invoke();
        RaiseOnGameEnded();
    }

}
