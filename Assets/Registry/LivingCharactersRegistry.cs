using System;
using System.Collections.Generic;
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
    }

    public void Dispose()
    {
        _characterSpawner.CharacterSpawned -= CharacterSpawned;
    }

    private void CharacterSpawned(CharacterFacade spawnedCharacter)
    {
        spawnedCharacter.OnDeath += RemoveDeadCharacter;
        LivingPlayers.Add(spawnedCharacter);
    }

    private void RemoveDeadCharacter(DeathParameters deathParameters)
    {
        var facade = LivingPlayers.Find(player => player.Id == deathParameters.characterInfo.Id);
        if(facade != null)
        {
            facade.OnDeath -= RemoveDeadCharacter;
            LivingPlayers.Remove(facade);
        }
    }
}