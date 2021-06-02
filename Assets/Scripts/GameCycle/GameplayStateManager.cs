using System;
using Zenject;

public class GameplayStateManager: IInitializable, IDisposable
{
    private GameState _state;
    private IGameCycle _gameCycle;

    public GameplayStateManager(
        GameState state,
        IGameCycle gameCycle)
    {
        _state = state;
        _gameCycle = gameCycle;
    }

    public void Initialize()
    {
        _gameCycle.OnGameStarted += resetGameState;    
    }


    public void Dispose()
    {
        _gameCycle.OnGameStarted -= resetGameState;
    }
    private void resetGameState()
    {
        _state.State = GameplayState.None;
    }
}