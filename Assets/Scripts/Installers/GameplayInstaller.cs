using DarkRift.Client.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private UnityClient _client;

    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromInstance(_mainCamera).AsSingle();
        Container.Bind<UnityClient>().FromInstance(_client).AsSingle();
    }
}
