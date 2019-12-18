using DarkRift;
using DarkRift.Client.Unity;
using System;
using System.Collections.Generic;
using Zenject;

public class NetworkedCharacterSpawner: IInitializable, IDisposable
{   
    private GenericMessageWithResponseClient _messageWithResponse;
    private GlobalHostPlayerManager _globalHostPlayerManager;
    private CharacterSpawner _characterSpawner;
    private NetworkRelay _networkRelay;
    private UnityClient _client;

    public NetworkedCharacterSpawner(
        GenericMessageWithResponseClient messageWithResponse,
        GlobalHostPlayerManager globalHostPlayerManager,
        CharacterSpawner characterSpawner,
        NetworkRelay networkRelay,
        UnityClient client
        )
    {
        _globalHostPlayerManager = globalHostPlayerManager;
        _messageWithResponse = messageWithResponse;
        _characterSpawner = characterSpawner;
        _networkRelay = networkRelay;
        _client = client;
    }

    public void Initialize()
    {
        _networkRelay.Subscribe(Tags.SpawnCharacter, HandleSpawnCharacter);
        _networkRelay.Subscribe(Tags.DespawnCharacter, HandleDespawn);
    }

    public void Dispose()
    {
        _networkRelay.Unsubscribe(Tags.SpawnCharacter, HandleSpawnCharacter);
        _networkRelay.Unsubscribe(Tags.DespawnCharacter, HandleDespawn);
    }

    private void HandleDespawn(Message message)
    {
        using (DarkRiftReader reader = message.GetReader())
        {
            //TODO MG CHECKSIZE
            ushort clientID = reader.ReadUInt16();
            _characterSpawner.Despawn(clientID);
        }
    }

    private void HandleSpawnCharacter(Message message)
    {  
        using (DarkRiftReader reader = message.GetReader())
        {
            //TODO MG CHECKSIZE
            while (reader.Position < reader.Length)
            {
                ushort id = reader.ReadUInt16();
                float X = reader.ReadSingle();
                float Y = reader.ReadSingle();
                bool isLocal = (id == _client.ID);

                CharacterSpawnParameters spawnParameters = new CharacterSpawnParameters();
                spawnParameters.Id = id;
                spawnParameters.X = X;
                spawnParameters.Y = Y;
                spawnParameters.playerId = id;
                spawnParameters.IsLocal = isLocal;
                spawnParameters.health = null;
                _characterSpawner.Spawn(spawnParameters);
            }
            _messageWithResponse.SendClientReady();
        }
    }

    public Message GenerateSpawnMessage()
    {
        var list = PrepareSpawnPositions();
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            foreach (PlayerSpawnData spawnData in list)
            {
                writer.Write(spawnData.Id);
                writer.Write(spawnData.X);
                writer.Write(spawnData.Y);
            }
            
            return Message.Create(Tags.SpawnCharacter, writer);            
        }
    }

    public Message GenerateDespawnMessage(ushort playerID)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(playerID);

            return Message.Create(Tags.DespawnCharacter, writer);
        }
    }

    private List<PlayerSpawnData> PrepareSpawnPositions()
    {

        //TODO MG: REMOVE ASAP: implement other method of determining positions.
        List<PlayerSpawnData> spawnPosisionsList = new List<PlayerSpawnData>();
        if (_globalHostPlayerManager.ConnectedPlayers.Count >= 1)
        {
            var spawnData = new PlayerSpawnData
            {
                Id = _globalHostPlayerManager.ConnectedPlayers[0],
                X = 0.5f,
                Y = 0.5f
            };
            spawnPosisionsList.Add(spawnData);
        }
        if (_globalHostPlayerManager.ConnectedPlayers.Count >= 2)
        {
            var spawnData = new PlayerSpawnData
            {
                Id = _globalHostPlayerManager.ConnectedPlayers[1],
                X = -0.5f,
                Y = 0.5f
            };
            spawnPosisionsList.Add(spawnData);
        }
        if (_globalHostPlayerManager.ConnectedPlayers.Count >= 3)
        {
            var spawnData = new PlayerSpawnData
            {
                Id = _globalHostPlayerManager.ConnectedPlayers[2],
                X = 0.5f,
                Y = -0.5f
            };
            spawnPosisionsList.Add(spawnData);
        }
        if (_globalHostPlayerManager.ConnectedPlayers.Count >= 4)
        {
            var spawnData = new PlayerSpawnData
            {
                Id = _globalHostPlayerManager.ConnectedPlayers[3],
                X = -0.5f,
                Y = -0.5f
            };
            spawnPosisionsList.Add(spawnData);
        }

        return spawnPosisionsList;
    }


}
