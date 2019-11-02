using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterSpawner : MonoBehaviour, IInitializable, IDisposable
{

    private UnityClient _client;
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
        Projectile.Factory projectileFactory
        )
    {
        _client = client;
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
        _playerFactory.Create(new CharacterSpawnParameters(0, new PlaceholderWeapon(_projectileFactory,new PlaceholderWeapon.Settings()), new Vector2(0, 0)));
        _client.MessageReceived += HandleMessage;
    }

    private void HandleMessage(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            if (message.Tag == Tags.SpawnCharacter) HandleSpawn(sender,e);
        }
    }

    private void HandleSpawn(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            using (DarkRiftReader reader = message.GetReader())
            {
                // check message size 

                // message reading
                ushort clientID = reader.ReadUInt16();
                IWeapon weapon = null; //some WeaponReading

                // message handling 

                if (_characters.ContainsKey(clientID) == false)
                {
                    // Generate Spawn coordinates 
                    // Should the position be generated on the server or by the client 
                    // (Probably server, he can ensure that all characters do not collide)
                    Vector2 position = new Vector2(0, 0);

                    CharacterSpawnParameters spawnParameters = new CharacterSpawnParameters(clientID, new PlaceholderWeapon(_projectileFactory, new PlaceholderWeapon.Settings()), position);
                    CharacterFacade characterFacade = _networkFactory.Create(spawnParameters);

                    _characters.Add(clientID, characterFacade);

                }

            }
        }
    }
}
