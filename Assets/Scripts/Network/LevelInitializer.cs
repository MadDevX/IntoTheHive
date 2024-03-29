﻿using System;
using UnityEngine;
using Zenject;

/// <summary>
/// This class is used to initialize game after changing scenes.
/// Methods from this class are executed when a project wide event is called from the other scene 
/// To make this class scene context based, an inter-scene event had to be introduced to trigger 
/// methods from this class without being bound at the time of a call to change scenes.
/// </summary>
public class LevelInitializer : IInitializable, IDisposable
{
    private GenericMessageWithResponseHost _messageWithResponse;
    private NetworkedCharacterSpawner _characterSpawner;
    private LevelGraphMessageSender _graphSender;
    private ProjectEventManager _eventManager;
    private HostDoorManager _hostDoorManager;

    public LevelInitializer(
        GenericMessageWithResponseHost messageWithResponse,
        NetworkedCharacterSpawner characterSpawner,
        LevelGraphMessageSender graphSender,
        ProjectEventManager eventManager,
        HostDoorManager hostDoorManager
        )
    {
        _messageWithResponse = messageWithResponse;
        _characterSpawner = characterSpawner;
        _graphSender = graphSender;
        _eventManager = eventManager;
        _hostDoorManager = hostDoorManager;
    }

    public void Initialize()
    {
        _eventManager.GameInitializedHost += LoadLevel;
    }

    public void Dispose()
    {
        _eventManager.GameInitializedHost -= LoadLevel;
    }

    private void LoadLevel()
    {
        _messageWithResponse.SendMessageWithResponse(_graphSender.GenerateLevelGraphMessage(), OpenSpawnRoom);
    }

    private void OpenSpawnRoom()
    {
        ScanGraphs();
        _messageWithResponse.SendMessageWithResponse(_hostDoorManager.PrepareOpenDoorsMessage(0), SpawnPlayers);
    }

    private void ScanGraphs()
    {
        AstarPath.active.Scan();
    }

    private void SpawnPlayers()
    {
        using (var message = _characterSpawner.GenerateSpawnMessage())
        {
            _messageWithResponse.SendMessageWithResponse(message, BeginGame);
        }
    }

    private void BeginGame()
    {
    }
}
