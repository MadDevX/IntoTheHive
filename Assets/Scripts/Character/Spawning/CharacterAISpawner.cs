using DarkRift.Client.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterAISpawner
{
    private CharacterFacade.Factory _AIfactory;
    private Dictionary<ushort, CharacterFacade> _AIplayers;
    private CharacterSpawner.Settings _settings;
    private ushort _nextId;

    [Inject]
    public void Construct(
        [Inject(Id = Identifiers.AI)] CharacterFacade.Factory AIFactory,
        CharacterSpawner.Settings settings
        )
    {
        _AIfactory = AIFactory;
        _settings = settings;
        _AIplayers = new Dictionary<ushort, CharacterFacade>();
        _nextId = (ushort)_settings.maxPlayers;
    }

    /// <summary>
    /// Spawns a character with given parameters.
    /// </summary>
    /// <param name="spawnParameters">Parameters to spawn a character.</param>
    public CharacterFacade Spawn(CharacterSpawnParameters spawnParameters)
    {
        ushort ID = spawnParameters.playerId;
        
        if (_AIplayers.ContainsKey(ID) == false)
        {
            CharacterFacade characterFacade = _AIfactory.Create(spawnParameters);
            characterFacade.Id = ID;
            characterFacade.CharacterType = CharacterType.AICharacter;
            // Put the character on spawn position
            Transform transform = characterFacade.transform;
            transform.SetPositionAndRotation(new Vector3(spawnParameters.X, spawnParameters.Y, transform.position.z), transform.rotation);
            _AIplayers.Add(ID, characterFacade);
            return characterFacade;
        }
        else
        {            
            return null;
        }
    }

    public void Despawn(ushort clientID)
    {
        if (_AIplayers.ContainsKey(clientID) == true)
        {
            var character = _AIplayers[clientID];
            _AIplayers.Remove(clientID);
            character.Dispose();
        }
    }

    public ushort GenerateNextID()
    {
        return ++_nextId;
    }

}
