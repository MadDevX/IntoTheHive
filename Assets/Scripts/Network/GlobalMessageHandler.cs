using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using System;
using Zenject;

class GlobalMessageHandler: IInitializable, IDisposable
{
    private UnityClient _client;
    private CharacterSpawner _characterSpawner;

    public GlobalMessageHandler(
        UnityClient client,
        CharacterSpawner characterSpawner)
    {
        _client = client;
        _characterSpawner = characterSpawner;
    }

    public void Initialize()
    {
        _client.MessageReceived += HandleMessage;
    }

    public void Dispose()
    {
        _client.MessageReceived -= HandleMessage;
    }

    private void HandleMessage(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            if (message.Tag == Tags.SpawnCharacter) HandleSpawn(sender, e);
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
                _characterSpawner.Despawn(clientID);
                
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
                while(reader.Position < reader.Length)
                {
                    ushort clientID = reader.ReadUInt16();
                    bool isLocal = reader.ReadBoolean();

                    CharacterSpawnParameters spawnParameters = new CharacterSpawnParameters();
                    _characterSpawner.Spawn(clientID, isLocal, spawnParameters);
                }
                // TODO message reading
            }
        }
    }

}
