using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterSpawner : IInitializable, IDisposable
{
    private CharacterFacade.Factory _networkFactory;
    private CharacterFacade.Factory _playerFactory;
    private CharacterFacade.Factory _AIfactory;
    private Projectile.Factory _projectileFactory;
    private CameraManager _cameraManager;
    private NetworkedCharacterSpawner _networkedCharacterSpawner;

    private Dictionary<ushort, CharacterFacade> _characters;

    [Inject]
    public void Construct(
        [Inject(Id = Identifiers.Network)] CharacterFacade.Factory networkFactory,
        [Inject(Id = Identifiers.AI)] CharacterFacade.Factory AIFactory,
        [Inject(Id = Identifiers.Player)] CharacterFacade.Factory playerFactory,
        NetworkedCharacterSpawner networkedCharacterSpawner,
        CameraManager cameraManager,
        Projectile.Factory projectileFactory
        )
    {
        _projectileFactory = projectileFactory;
        _networkFactory = networkFactory;
        _playerFactory = playerFactory;
        _AIfactory = AIFactory;

        _cameraManager = cameraManager;
        _networkedCharacterSpawner = networkedCharacterSpawner;
        _characters = new Dictionary<ushort, CharacterFacade>();

    }


    public void Initialize()
    {
        _networkedCharacterSpawner.PlayerSpawned += Spawn;
        _networkedCharacterSpawner.PlayerDespawned += Despawn;
    }

    public void Dispose()
    {
        _networkedCharacterSpawner.PlayerSpawned -= Spawn;
        _networkedCharacterSpawner.PlayerDespawned -= Despawn;
    }

    //isLocal corresponds to whether the client to be spawned is Local or not
    public void Spawn(CharacterSpawnParameters spawnParameters)
    {
        ushort clientID = spawnParameters.SenderId;
        bool isLocal = spawnParameters.IsLocal;
        if (_characters.ContainsKey(clientID) == false)
        {
            // Generate Spawn coordinates 
            // Should the position be generated on the server or by the client?
            // (Probably server, he can ensure that all characters do not collide)
            
            if(isLocal)
            {
                CharacterFacade characterFacade = _playerFactory.Create(spawnParameters);
                _cameraManager.SetCameraToPlayerCharacter(characterFacade);
                _characters.Add(clientID, characterFacade);
            }
            else
            {
                CharacterFacade characterFacade = _networkFactory.Create(spawnParameters);
                _characters.Add(clientID, characterFacade);
            }
        }
    }

    public void SpawnAI(CharacterSpawnParameters spawnParameters)
    {
        ushort clientID = spawnParameters.SenderId;
        bool isLocal = spawnParameters.IsLocal;
        if (_characters.ContainsKey(clientID) == false)
        {
            // Generate Spawn coordinates 
            // Should the position be generated on the server or by the client?
            // (Probably server, he can ensure that all characters do not collide)

            CharacterFacade AICharacterFacade = _AIfactory.Create(spawnParameters);
            _characters.Add(clientID, AICharacterFacade);
        }

        
    }

    public void Despawn(ushort clientID)
    {
        if (_characters.ContainsKey(clientID) == true)
        {
            var character = _characters[clientID];
            _characters.Remove(clientID);
            character.Dispose();
        }
    }

}
