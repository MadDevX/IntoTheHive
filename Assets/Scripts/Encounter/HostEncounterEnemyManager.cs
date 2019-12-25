using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

/// <summary>
/// Handles spawning encoutner enemies along with spawning them locally on host and sending spawn messages.
/// </summary>
public class HostEncounterEnemyManager: IDisposable
{
    public event Action AllEnemiesDead;

    private event Action WaveCleared;
    private List<CharacterFacade> _aliveEnemies = new List<CharacterFacade>();    
    
    private GenericMessageWithResponseHost _synchronizedMessageSender;
    private NetworkedAISpawner _networkedAISpawner;
    private RoomEnemyInfoPool _enemyInfoPool;
    private CharacterAISpawner _aiSpawner;

    public HostEncounterEnemyManager(
        GenericMessageWithResponseHost synchronizedMessageSender,
        NetworkedAISpawner networkedAISpawner,
        RoomEnemyInfoPool enemyInfoPool,
        CharacterAISpawner aiSpawner
        )
    {
        _synchronizedMessageSender = synchronizedMessageSender;
        _networkedAISpawner = networkedAISpawner;
        _enemyInfoPool = enemyInfoPool;
        _aiSpawner = aiSpawner;
    }
    
    [Inject]
    public void Initialize()
    {
        WaveCleared += SpawnEnemies;
    }

    public void Dispose()
    {
        WaveCleared -= SpawnEnemies;
    }

    /// <summary>
    /// Spawns the next wave of enemies, both locally on host and on all clients through messages.
    /// </summary>
    public void SpawnEnemies()
    {
        // Get data from spawner list
        var data = _enemyInfoPool.GetEnemySpawnInfos();
        
        // Check if data.Count is greater than zero
        if(data.Count > 0)
        {
            // Spawn enemies locally and through network
            // Generate an unique id for each enemy
            data.ForEach(enemy => enemy.SpawnParameters.Id = _aiSpawner.GenerateNextID());
            _synchronizedMessageSender.SendMessageWithResponse(_networkedAISpawner.GenerateSpawnMessage(data));
            data.ForEach(enemy => enemy.SpawnParameters.IsLocal = true);
            LocalSpawn(data);
        }
        else
        {
            // All waves completed
            AllEnemiesDead?.Invoke();
        }                
    }

    /// <summary>
    /// Spawns enemies on the host side.
    /// </summary>
    /// <param name="data"> Data on which to spawn enemies. </param>
    /// <returns> Wheter any </returns>
    private void LocalSpawn(List<AISpawnParameters> data)
    {
        data.ForEach(enemy =>
        {
            CharacterFacade facade = _aiSpawner.Spawn(enemy.SpawnParameters);
            if (facade == null) throw new Exception("Encountered error while spawning enemies. Character facade is null.");
            _aliveEnemies.Add(facade);
            facade.OnDeath += OnAIDeath;
        });
    }

    /// <summary>
    /// Remove local enemies and 
    /// </summary>
    /// <param name="deathParameters"></param>
    private void OnAIDeath(DeathParameters deathParameters)
    {
        CharacterFacade facade = _aliveEnemies.Find(enemy => enemy.Id == deathParameters.characterInfo.Id);
        Debug.Log("death parameters id = " + deathParameters.characterInfo.Id);
        //Unsubsribe event
        if (facade == null) Debug.Log("Facade is null");
        facade.OnDeath -= OnAIDeath;

        _aliveEnemies.Remove(facade);
        //AllEnemiesDead?.Invoke();
        CheckWaveEnded();
    }

    /// <summary>
    /// Checks wheter the wave ended after all clients respond that they despawned their AI enemies.
    /// </summary>
    private void CheckWaveEnded()
    {
        if (_aliveEnemies.Count == 0)
        {
            WaveCleared?.Invoke();
        }
    }
}