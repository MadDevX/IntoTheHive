using DarkRift.Client.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterSpawner
{
    public event Action<CharacterFacade> CharacterSpawned;
    public event Action<CharacterFacade> CharacterDespawned;

    private CharacterFacade.Factory _networkFactory;
    private CharacterFacade.Factory _playerFactory;
    private CameraManager _cameraManager;
    private Dictionary<ushort, CharacterFacade> _characters;
    private UnityClient _unityClient;

    [Inject]
    public void Construct(
        [Inject(Id = Identifiers.Network)] CharacterFacade.Factory networkFactory,
        [Inject(Id = Identifiers.AI)] CharacterFacade.Factory AIFactory,
        [Inject(Id = Identifiers.Player)] CharacterFacade.Factory playerFactory,
        CameraManager cameraManager,
        UnityClient unityClient
        )
    {
        _networkFactory = networkFactory;
        _playerFactory = playerFactory;
        _unityClient = unityClient;
        _cameraManager = cameraManager;
        _characters = new Dictionary<ushort, CharacterFacade>();
    }

    /// <summary>
    /// Spawns a character with given parameters.
    /// </summary>
    /// <param name="spawnParameters">Parameters to spawn a character.</param>
    public void Spawn(CharacterSpawnParameters spawnParameters)
    {
        ushort playerId = spawnParameters.Id;
        bool isLocal = spawnParameters.IsLocal;

        if (_characters.ContainsKey(playerId) == false)
        {
            CharacterFacade characterFacade = null;
            if (isLocal)
            {                
                // Player character
                characterFacade = _playerFactory.Create(spawnParameters);                
                characterFacade.Id = _unityClient.ID;
                _cameraManager.SetCameraToPlayerCharacter(characterFacade);
            }
            else
            {
                // Networked character 
                characterFacade = _networkFactory.Create(spawnParameters);
                characterFacade.Id = playerId;
                // TODO CHANGE WHEN REPLACING SPRITES
                if (characterFacade.CharacterType == CharacterType.AICharacter)
                {
                    var renderer = characterFacade.GetComponentInChildren<SpriteRenderer>();
                    renderer.color = Color.green;
                }
            }

            // Put the character on spawn position
            Transform transform = characterFacade.transform;
            transform.SetPositionAndRotation(new Vector3(spawnParameters.X, spawnParameters.Y, transform.position.z), transform.rotation);
            _characters.Add(playerId, characterFacade);
            CharacterSpawned?.Invoke(characterFacade);
        }
    }

    /// <summary>
    /// Despawns a character
    /// </summary>
    /// <param name="clientID"></param>
    public void Despawn(ushort clientID)
    {
        if (_characters.ContainsKey(clientID) == true)
        {
            var character = _characters[clientID];
            _characters.Remove(clientID);
            character.Dispose();
            CharacterDespawned?.Invoke(character);
        }
    }

    //TODO MG FIGURE OUT IF THIS SHOULD BE HERE 
    // IS USED IN NETWORKEDAISPAWNER TO GET AI Ids

    [System.Serializable]
    public class Settings
    {
        public int maxPlayers = 4;
    }
}
