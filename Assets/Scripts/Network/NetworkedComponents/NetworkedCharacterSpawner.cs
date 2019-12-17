using DarkRift;
using DarkRift.Client.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NetworkedCharacterSpawner: IInitializable, IDisposable
{
    private UnityClient _client;
    private NetworkRelay _networkRelay;
    private GlobalHostPlayerManager _globalHostPlayerManager;
    private GenericMessageWithResponseHost _messageWithResponse;
    public event Action<ushort> PlayerDespawned;
    public event Action<CharacterSpawnParameters> PlayerSpawned;


    private LevelGraphMessageSender _graphSender;

    public NetworkedCharacterSpawner(
        GenericMessageWithResponseHost messageWithResponse,
        GlobalHostPlayerManager globalHostPlayerManager,
        UnityClient client,
        LevelGraphMessageSender sender,
        NetworkRelay networkRelay
        )
    {
        _messageWithResponse = messageWithResponse;
        _graphSender = sender;
        _globalHostPlayerManager = globalHostPlayerManager;
        _client = client;
        _networkRelay = networkRelay;
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
            PlayerDespawned?.Invoke(clientID);
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
                // Replace with id == _client.ID

                CharacterSpawnParameters spawnParameters = new CharacterSpawnParameters();
                spawnParameters.Id = id;
                spawnParameters.X = X;
                spawnParameters.Y = Y;
                // spawnParameters.SenderId = TODO MG
                spawnParameters.SenderId = id;
                spawnParameters.IsLocal = isLocal;
                spawnParameters.health = new CharacterHealth();
                PlayerSpawned?.Invoke(spawnParameters);
            }
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

    private List<PlayerSpawnData> PrepareSpawnPositions()
    {

        //TODO MG: REMOVE ASAP: implement other method of determining positions.
        List<PlayerSpawnData> spawnPosisionsList = new List<PlayerSpawnData>();
        if (_globalHostPlayerManager.ConnectedPlayers.Count >= 1)
        {
            var spawnData = new PlayerSpawnData();
            spawnData.Id = _globalHostPlayerManager.ConnectedPlayers[0];
            spawnData.X = 0.5f;
            spawnData.Y = 0.5f;
            spawnPosisionsList.Add(spawnData);
        }
        if (_globalHostPlayerManager.ConnectedPlayers.Count >= 2)
        {
            var spawnData = new PlayerSpawnData();
            spawnData.Id = _globalHostPlayerManager.ConnectedPlayers[1];
            spawnData.X = -0.5f;
            spawnData.Y = 0.5f;
            spawnPosisionsList.Add(spawnData);
        }
        if (_globalHostPlayerManager.ConnectedPlayers.Count >= 3)
        {
            var spawnData = new PlayerSpawnData();
            spawnData.Id = _globalHostPlayerManager.ConnectedPlayers[2];
            spawnData.X = 0.5f;
            spawnData.Y = -0.5f;
            spawnPosisionsList.Add(spawnData);
        }
        if (_globalHostPlayerManager.ConnectedPlayers.Count >= 4)
        {
            var spawnData = new PlayerSpawnData();
            spawnData.Id = _globalHostPlayerManager.ConnectedPlayers[3];
            spawnData.X = -0.5f;
            spawnData.Y = -0.5f;
            spawnPosisionsList.Add(spawnData);
        }

        return spawnPosisionsList;
    }


}
