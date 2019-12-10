﻿using DarkRift;
using DarkRift.Client.Unity;
using System;
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

    public void InitiateSpawn()
    {
        Debug.Log("Spawn Initiated");

        //_graphSender.SendLevelGraph();
        _messageWithResponse.SendMessageWithResponse(Tags.LevelGraph, _graphSender.SendLevelGraph, SpawnAll);
        //SpawnAll();
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
                bool isLocal = (id == _client.ID);
                // Replace with id == _client.ID

                CharacterSpawnParameters spawnParameters = new CharacterSpawnParameters();
                spawnParameters.Id = id;
                // spawnParameters.SenderId = TODO MG
                spawnParameters.SenderId = id;
                spawnParameters.IsLocal = isLocal;
                PlayerSpawned?.Invoke(spawnParameters);
            }
        }
    }

    private void SpawnAll()
    {
        PrepareSpawnPositions();
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            foreach (ushort playerId in _globalHostPlayerManager.ConnectedPlayers)
            {
                writer.Write(playerId);
            }
            
            using (Message message = Message.Create(Tags.SpawnCharacter, writer))
            {
                _client.SendMessage(message,SendMode.Reliable);
            }
        }
    }

    private void PrepareSpawnPositions()
    {
        //throw new NotImplementedException();
    }

}
