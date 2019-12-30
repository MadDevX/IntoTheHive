using Cinemachine;
using DarkRift.Client.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private Camera _mainCamera; 
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private SceneInitializedAnnouncer _announcer;


    public override void InstallBindings()
    {       
        InstallCameras();
        InstallInitializationHandling();
        InstallSpawning();
        InstallItems();
    }

    private void InstallCameras()
    {
        Container.Bind<Camera>().FromInstance(_mainCamera).AsSingle();
        Container.Bind<CinemachineVirtualCamera>().FromInstance(_virtualCamera).AsSingle();
        Container.BindInterfacesAndSelfTo<CameraManager>().AsSingle();
    }

    private void InstallInitializationHandling()
    {
        Container.BindInstance(_announcer).AsSingle();
        Container.BindInterfacesAndSelfTo<SceneInitializedBaseHandler>().AsSingle();
    }
    private void InstallSpawning()
    {
        Container.BindInterfacesAndSelfTo<CharacterSpawner>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterAISpawner>().AsSingle();
        Container.BindInterfacesAndSelfTo<NetworkedCharacterSpawner>().AsSingle();
        Container.BindInterfacesAndSelfTo<NetworkedAISpawner>().AsSingle();
        Container.BindInterfacesAndSelfTo<ItemInitializer>().AsSingle();
    }

    private void InstallItems()
    {
        Container.BindInterfacesAndSelfTo<ModuleFactory>().AsSingle().When(x => x.Container == Container);
        Container.BindInterfacesAndSelfTo<ItemFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<WeaponCreator>().AsSingle();
        Container.BindInterfacesAndSelfTo<PickupManager>().AsSingle();
    }

}
