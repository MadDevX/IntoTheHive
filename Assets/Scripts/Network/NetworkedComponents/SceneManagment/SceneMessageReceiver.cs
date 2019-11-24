﻿using DarkRift;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneMessageReceiver: IInitializable,IDisposable
{
    private Scenes _scenes;
    private NetworkRelay _relay;
    private SceneMessageSender _messageSender;

    // TODO MG figure out if this scenes should be stored here somehow
    private Dictionary<ushort, SceneReference> _sceneReferences;

    public SceneMessageReceiver(
        Scenes scenes,
        NetworkRelay relay,
        SceneMessageSender messageSender
        )
    {
        _scenes = scenes;
        _relay = relay;
        _messageSender = messageSender;
        _sceneReferences = new Dictionary<ushort, SceneReference>();
    }

    public void Initialize()
    {
        _relay.Subscribe(Tags.ChangeScene, ParseChangeSceneMessage);
        _relay.Subscribe(Tags.ChangeSceneWithReply, ParseChangeSceneWithResponseMessage);
    }

    public void Dispose()
    {
        _relay.Unsubscribe(Tags.ChangeScene, ParseChangeSceneMessage);
        _relay.Unsubscribe(Tags.ChangeSceneWithReply, ParseChangeSceneWithResponseMessage);
    }  

    private void ParseChangeSceneMessage(Message message)
    {
        int sceneBuildIndex;

        using (DarkRiftReader reader = message.GetReader())
        {
            sceneBuildIndex = reader.ReadUInt16();
        }

        var asyncOperation = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Single);
    }

    // TODO MG : move this method to sceneChenagedwithResponseSender
    private void ParseChangeSceneWithResponseMessage(Message message)
    {
        int sceneBuildIndex;

        using (DarkRiftReader reader = message.GetReader())
        {
            sceneBuildIndex = reader.ReadUInt16();
        }

        // TODO MG : delete this and send SceneReady on SceneInitialized
        var asyncOperation = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Single);
        asyncOperation.completed += SceneReady;
    }

    // TODO MG : move this method to sceneChenagedwithResponseSender
    private void SceneReady(AsyncOperation obj)
    { 
        _messageSender.SendSceneReady();
    }    
}

