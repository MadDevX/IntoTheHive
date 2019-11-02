using DarkRift.Client.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GlobalNetworkInstaller : MonoInstaller
{
    [SerializeField] private UnityClient _client;
    [SerializeField] private CharacterSpawner _characterSpawner;

    public override void InstallBindings()
    {
        Container.Bind<UnityClient>().FromInstance(_client).AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterSpawner>().FromInstance(_characterSpawner).AsSingle();
    }
}
