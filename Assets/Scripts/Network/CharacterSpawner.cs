using Cinemachine;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterSpawner : MonoBehaviour, IInitializable, IDisposable
{
    //Separate this to CharacterSpawner and NetworkHandler? to separate message handling from spawning players 
    private UnityClient _client;

    private CinemachineVirtualCamera _camera;
    private CharacterFacade.Factory _networkFactory;
    private CharacterFacade.Factory _playerFactory;
    private CharacterFacade.Factory _AIfactory;
    private Projectile.Factory _projectileFactory;

    private Dictionary<ushort, CharacterFacade> _characters;

    [Inject]
    public void Construct(
        UnityClient client,
        [Inject(Id = Identifiers.Network)] CharacterFacade.Factory networkFactory,
        [Inject(Id = Identifiers.AI)] CharacterFacade.Factory AIFactory,
        [Inject(Id = Identifiers.Player)] CharacterFacade.Factory playerFactory,
        Projectile.Factory projectileFactory,
        CinemachineVirtualCamera camera
        )
    {
        _client = client;
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
        CharacterFacade playerCharacter = _playerFactory.Create(new CharacterSpawnParameters());
        _camera.Follow = playerCharacter.transform;
        _camera.LookAt = playerCharacter.transform;
        _client.MessageReceived += HandleMessage;
    }

    private void HandleMessage(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            if (message.Tag == Tags.SpawnCharacter) HandleSpawn(sender,e);
            if (message.Tag == Tags.DespawnCharacter) HandleDespawn(sender, e);
        }
    }

    private void HandleDespawn(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            using (DarkRiftReader reader = message.GetReader())
            {
                // TODO check message size 
                ushort clientID = reader.ReadUInt16();

                if (_characters.ContainsKey(clientID) == true)
                {
                    var character = _characters[clientID];
                    _characters.Remove(clientID);
                    character.Dispose();                    
                }
            }
        }
    }

    private void HandleSpawn(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            using (DarkRiftReader reader = message.GetReader())
            {
                // TODO  check message size 

                // TODO message reading
                ushort clientID = reader.ReadUInt16();

                if (_characters.ContainsKey(clientID) == false)
                {
                    // Generate Spawn coordinates 
                    // Should the position be generated on the server or by the client?
                    // (Probably server, he can ensure that all characters do not collide)

                    CharacterSpawnParameters spawnParameters = new CharacterSpawnParameters();
                    CharacterFacade characterFacade = _networkFactory.Create(spawnParameters);
                    _characters.Add(clientID, characterFacade);

                }

            }
        }
    }
}
