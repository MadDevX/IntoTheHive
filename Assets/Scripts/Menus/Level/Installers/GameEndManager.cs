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
    public GameEndManager(
        LivingCharactersRegistry livingCharactersRegistry,
        HostSceneManager sceneManager,
        ClientInfo info,
        IGameCycleController cycleController)
    {
        _livingCharactersRegistry = livingCharactersRegistry;
        _sceneManager = sceneManager;
        _info = info;
        _cycleController = cycleController;
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
        _sceneManager.LoadLobby();
        _cycleController.RaiseOnGameEnded();
    }

}