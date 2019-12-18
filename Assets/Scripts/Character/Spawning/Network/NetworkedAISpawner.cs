using DarkRift;
using DarkRift.Client.Unity;
using System;
using System.Collections.Generic;
using Zenject;

public class NetworkedAISpawner : IInitializable, IDisposable
{    
    private GenericMessageWithResponseClient _messageWithResponse;
    private GlobalHostPlayerManager _globalHostPlayerManager;
    private CharacterAISpawner _AISpawner;
    private NetworkRelay _networkRelay;

    public NetworkedAISpawner(
        GenericMessageWithResponseClient messageWithResponse,
        GlobalHostPlayerManager globalHostPlayerManager,
        CharacterAISpawner characterSpawner,
        NetworkRelay networkRelay)
    {
        _globalHostPlayerManager = globalHostPlayerManager;
        _messageWithResponse = messageWithResponse;
        _AISpawner = characterSpawner;
        _networkRelay = networkRelay;
    }

    public void Initialize()
    {
        _networkRelay.Subscribe(Tags.SpawnAI, HandleSpawn);
        _networkRelay.Subscribe(Tags.DespawnAI, HandleDespawn);
    }

    public void Dispose()
    {
        _networkRelay.Unsubscribe(Tags.SpawnAI, HandleSpawn);
        _networkRelay.Unsubscribe(Tags.DespawnAI, HandleDespawn);
    }

    private void HandleDespawn(Message message)
    {
        using (DarkRiftReader reader = message.GetReader())
        {
            //TODO MG CHECKSIZE
            ushort clientID = reader.ReadUInt16();
            _AISpawner.Despawn(clientID);
        }
    }

    private void HandleSpawn(Message message)
    {  
        using (DarkRiftReader reader = message.GetReader())
        {
            //TODO MG CHECKSIZE
            while (reader.Position < reader.Length)
            {
                ushort id = reader.ReadUInt16();
                float X = reader.ReadSingle();
                float Y = reader.ReadSingle();
                CreateAIPlayer(id, X, Y);                
            }
            _messageWithResponse.SendClientReady();
        }
    }

    public void CreateAIPlayer(ushort id, float X, float Y)
    {
        CharacterSpawnParameters spawnParameters = new CharacterSpawnParameters
        {
            Id = id,
            X = X,
            Y = Y,
            playerId = id,
            health = null
        };
        _AISpawner.Spawn(spawnParameters);
    }

    public Message GenerateSpawnMessage(float X, float Y)
    {
        var list = PrepareSpawnPositions(X,Y);
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            foreach (PlayerSpawnData spawnData in list)
            {
                writer.Write(_AISpawner.GenerateNextID());
                writer.Write(spawnData.X);
                writer.Write(spawnData.Y);
            }
            
            return Message.Create(Tags.SpawnCharacter, writer);            
        }
    }   

    private List<PlayerSpawnData> PrepareSpawnPositions(float X, float Y)
    {
        //TODO MG: REMOVE ASAP: implement other method of determining positions.
        List<PlayerSpawnData> spawnPosisionsList = new List<PlayerSpawnData>();
        if (_globalHostPlayerManager.ConnectedPlayers.Count >= 1)
        {
            var spawnData = new PlayerSpawnData
            {
                Id = _globalHostPlayerManager.ConnectedPlayers[0],
                X = X + 0.5f,
                Y = Y + 0.5f
            };
            spawnPosisionsList.Add(spawnData);
        }
        if (_globalHostPlayerManager.ConnectedPlayers.Count >= 2)
        {
            var spawnData = new PlayerSpawnData
            {
                Id = _globalHostPlayerManager.ConnectedPlayers[1],
                X = X - 0.5f,
                Y = Y + 0.5f
            };
            spawnPosisionsList.Add(spawnData);
        }
        if (_globalHostPlayerManager.ConnectedPlayers.Count >= 3)
        {
            var spawnData = new PlayerSpawnData
            {
                Id = _globalHostPlayerManager.ConnectedPlayers[2],
                X = X + 0.5f,
                Y = Y - 0.5f
            };
            spawnPosisionsList.Add(spawnData);
        }
        if (_globalHostPlayerManager.ConnectedPlayers.Count >= 4)
        {
            var spawnData = new PlayerSpawnData
            {
                Id = _globalHostPlayerManager.ConnectedPlayers[3],
                X = X - 0.5f,
                Y = Y - 0.5f
            };
            spawnPosisionsList.Add(spawnData);
        }

        return spawnPosisionsList;
    }


}
