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
    public GameEndManager(
        LivingCharactersRegistry livingCharactersRegistry,
        HostSceneManager sceneManager,
        ClientInfo info)
    {
        _livingCharactersRegistry = livingCharactersRegistry;
        _sceneManager = sceneManager;
        _info = info;
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
        Debug.LogWarning("Game Ending");
        _sceneManager.LoadLobby();
    }

}