using Cinemachine;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterSpawner : IInitializable, IDisposable
{
    
    private CinemachineVirtualCamera _camera;
    private CharacterFacade.Factory _networkFactory;
    private CharacterFacade.Factory _playerFactory;
    private CharacterFacade.Factory _AIfactory;
    private Projectile.Factory _projectileFactory;

    private Dictionary<ushort, CharacterFacade> _characters;

    [Inject]
    public void Construct(
        [Inject(Id = Identifiers.Network)] CharacterFacade.Factory networkFactory,
        [Inject(Id = Identifiers.AI)] CharacterFacade.Factory AIFactory,
        [Inject(Id = Identifiers.Player)] CharacterFacade.Factory playerFactory,
        Projectile.Factory projectileFactory,
        CinemachineVirtualCamera camera
        )
    {
        _camera = camera;
        _networkFactory = networkFactory;
        _playerFactory = playerFactory;
        _AIfactory = AIFactory;
        _projectileFactory = projectileFactory;
        _characters = new Dictionary<ushort, CharacterFacade>();
    }

    public void Dispose()
    {
        //Add dispose Handling
    }

    public void Initialize()
    {
    }

    //isLocal corresponds to whether the client to be spawned is Local or not
    public void Spawn(ushort clientID, bool isLocal, CharacterSpawnParameters spawnParameters)
    {
        if (_characters.ContainsKey(clientID) == false)
        {
            // Generate Spawn coordinates 
            // Should the position be generated on the server or by the client?
            // (Probably server, he can ensure that all characters do not collide)
            
            if(isLocal)
            {
                CharacterFacade characterFacade = _playerFactory.Create(spawnParameters);
                _camera.Follow = characterFacade.transform;
                _characters.Add(clientID, characterFacade);
            }
            else
            {
                CharacterFacade characterFacade = _networkFactory.Create(spawnParameters);
                _characters.Add(clientID, characterFacade);
            }
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

    public void InitiateSpawn()
    {

        Debug.Log("Spawn Initiated");
        PrepareSpawnPositions();
        SpawnAll();

    }

    private void PrepareSpawnPositions()
    {
        throw new NotImplementedException();
    }

    private void SpawnAll()
    {
        throw new NotImplementedException();
    }
}
