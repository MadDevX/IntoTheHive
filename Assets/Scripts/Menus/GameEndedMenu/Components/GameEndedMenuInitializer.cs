using DarkRift;
using DarkRift.Client.Unity;
using System;
using UnityEngine;
using Zenject;

/// <summary>
/// Handles PostInitialization logic for GameEndedMenu. Sends an information wheter player won or not.
/// </summary>
public class GameEndedMenuInitializer : IInitializable, IDisposable
{
    private GameState _state;
    private ProjectEventManager _projectEventManager;
    private UnityClient _client;
    public GameEndedMenuInitializer(
        GameState state,
        ProjectEventManager projectEventManager,
        UnityClient client)
    {
        _state = state;
        _projectEventManager = projectEventManager;
        _client = client;
    }

    public void Initialize()
    {
        _projectEventManager.GameEndedMenuInitializedHost += SendGameState;
    }

    public void Dispose()
    {
        _projectEventManager.GameEndedMenuInitializedHost -= SendGameState;
    }

    /// <summary>
    /// Sends an information whether the game is won/lost to all clients.
    /// </summary>
    public void SendGameState()
    {        
        using(DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(_state.State == GameplayState.Win);
            using(Message gameStateUpdate = Message.Create(Tags.UpdateGameState,writer))
            {
                _client.SendMessage(gameStateUpdate, SendMode.Reliable);
            }
        }
    }
}