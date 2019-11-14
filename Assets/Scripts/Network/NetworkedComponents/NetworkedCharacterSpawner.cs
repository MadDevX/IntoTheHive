using DarkRift;
using DarkRift.Client.Unity;
using System;
using Zenject;

class NetworkedCharacterSpawner: IInitializable, IDisposable
{
    private UnityClient _client;
    private CharacterSpawner _characterSpawner;
    private NetworkRelay _networkRelay;

    public NetworkedCharacterSpawner(
        UnityClient client,
        NetworkRelay networkRelay,
        CharacterSpawner characterSpawner)
    {
        _client = client;
        _networkRelay = networkRelay;
        _characterSpawner = characterSpawner;
    }

    public void Initialize()
    {
        _networkRelay.Subscribe(Tags.SpawnCharacter, HandleSpawn);
        _networkRelay.Subscribe(Tags.DespawnCharacter, HandleDespawn);
    }

    public void Dispose()
    {
        _networkRelay.Unsubscribe(Tags.SpawnCharacter, HandleSpawn);
        _networkRelay.Unsubscribe(Tags.DespawnCharacter, HandleDespawn);
    }

    private void HandleDespawn(Message message)
    {
        using (DarkRiftReader reader = message.GetReader())
        {
            // TODO check message size 
            ushort clientID = reader.ReadUInt16();
            _characterSpawner.Despawn(clientID);
        }
    }

    private void HandleSpawn(Message message)
    {  
        using (DarkRiftReader reader = message.GetReader())
        {
            // TODO  check message size 
            while(reader.Position < reader.Length)
            {
                ushort id = reader.ReadUInt16();
                bool isLocal = reader.ReadBoolean();

                CharacterSpawnParameters spawnParameters = new CharacterSpawnParameters();
                spawnParameters.Id = id;
                _characterSpawner.Spawn(id, isLocal, spawnParameters);
            }
        }
    }
}
