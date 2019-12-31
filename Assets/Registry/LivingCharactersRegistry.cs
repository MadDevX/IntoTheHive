using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LivingCharactersRegistry: IInitializable, IDisposable
{
    public int LivingPlayersCount
    {
        get
        {
            return LivingPlayers.Count;
        }
    }

    public List<CharacterFacade> LivingPlayers = new List<CharacterFacade>();
    public event Action AllPlayersDead;

    private CharacterSpawner _characterSpawner;

    public LivingCharactersRegistry(CharacterSpawner characterSpawner)
    {
        _characterSpawner = characterSpawner;
    }

    public void Initialize()
    {
        _characterSpawner.CharacterSpawned += CharacterSpawned;
        _characterSpawner.CharacterDespawned += CharacterDespawned;
    }

    public void Dispose()
    {
        _characterSpawner.CharacterSpawned -= CharacterSpawned;
        _characterSpawner.CharacterDespawned -= CharacterDespawned;
    }

    private void CharacterDespawned(CharacterFacade despawnedCharacter)
    {
        Remove(despawnedCharacter);
    }

    private void CharacterSpawned(CharacterFacade spawnedCharacter)
    {
        spawnedCharacter.OnDeath += RemoveDeadCharacter;
        LivingPlayers.Add(spawnedCharacter);
    }
    
    public void RemoveDeadCharacter(ushort id)
    {
        var facade = LivingPlayers.Find(player => player.Id == id);
        if (facade != null)
        {
            facade.OnDeath -= RemoveDeadCharacter;
            Remove(facade);
        }
    }

    private void RemoveDeadCharacter(DeathParameters deathParameters)
    {
        var facade = LivingPlayers.Find(player => player.Id == deathParameters.characterInfo.Id);
        if(facade != null)
        {
            facade.OnDeath -= RemoveDeadCharacter;
            Remove(facade);
        }
    }

    private void Remove(CharacterFacade facade)
    {
        LivingPlayers.Remove(facade);
        if (LivingPlayersCount == 0)
            AllPlayersDead?.Invoke();
    }
}