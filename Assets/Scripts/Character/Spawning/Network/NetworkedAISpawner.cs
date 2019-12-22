using DarkRift;
using DarkRift.Client.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
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

    public Message GenerateSpawnMessage(List<AISpawnParameters> data)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            foreach (AISpawnParameters spawnData in data)
            {
                writer.Write(spawnData.SpawnParameters.Id);
                writer.Write(spawnData.SpawnParameters.X);
                writer.Write(spawnData.SpawnParameters.Y);
            }
            
            return Message.Create(Tags.SpawnAI, writer);            
        }
    }   

    public Message GenerateDespawnMessage(ushort playerID)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(playerID);

            return Message.Create(Tags.DespawnAI, writer);
        }
    }
}
