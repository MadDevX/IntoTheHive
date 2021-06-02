using System;
using UnityEngine;
using Zenject;
/// <summary>
/// Ends the game by loading the lobby for each player.
/// </summary>
public class GameEndManager: IInitializable, IDisposable
{
    private LivingCharactersRegistry _livingCharactersRegistry;
    private HostSceneManager _sceneManager;
    private ClientInfo _info;
    private IGameCycleController _cycleController;
    private GameState _state;

    public GameEndManager(
        LivingCharactersRegistry livingCharactersRegistry,
        HostSceneManager sceneManager,
        ClientInfo info,
        IGameCycleController cycleController,
        GameState state)
    {
        _livingCharactersRegistry = livingCharactersRegistry;
        _sceneManager = sceneManager;
        _info = info;
        _cycleController = cycleController;
        _state = state;
    }

    public void Initialize()
    {
        if(_info.Status == ClientStatus.Host)   
            _livingCharactersRegistry.AllPlayersDead += EndGame;
    }

    public void Dispose()
    {
        if (_info.Status == ClientStatus.Host)
            _livingCharactersRegistry.AllPlayersDead -= EndGame;
    }

    private void EndGame()
    {
        _state.State = GameplayState.Lose;
        _sceneManager.LoadGameEndedMenu();
        _cycleController.RaiseOnGameEnded();
    }

}