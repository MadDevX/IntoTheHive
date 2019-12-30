using System;

public interface IGameCycle
{
    event Action OnGameStarted;
    event Action OnGameEnded;
}