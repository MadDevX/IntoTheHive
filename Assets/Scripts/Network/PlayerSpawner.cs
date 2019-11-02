using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The DarkRift client to communicate on.")]
    private UnityClient _client;
    private CharacterFacade.Factory _networkFactory;
    private CharacterFacade.Factory _playerFactory;
    private CharacterFacade.Factory _AIfactory;
    private Dictionary<ushort, CharacterFacade> _characters;


    public PlayerSpawner(
        UnityClient client, 
        [Inject(Id = Identifiers.Network)]CharacterFacade.Factory networkFactory,
        [Inject(Id = Identifiers.AI)]CharacterFacade.Factory AIFactory,
        [Inject(Id = Identifiers.Player)]CharacterFacade.Factory playerFactory)
    {
        _client = client;
        _networkFactory = networkFactory;
        _playerFactory = playerFactory;
        _AIfactory = AIFactory;
        _characters = new Dictionary<ushort, CharacterFacade>();
    }


    void Awake()
    {
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

                    //insert characterSpawnParameters somehow
                    CharacterSpawnParameters spawnParameters = new CharacterSpawnParameters(clientID,weapon,position);
                    CharacterFacade characterFacade = _networkFactory.Create(spawnParameters);

                    _characters.Add(clientID, characterFacade);

                }

            }
        }
    }
}
