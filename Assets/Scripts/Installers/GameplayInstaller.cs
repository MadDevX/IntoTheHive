using Cinemachine;
using DarkRift.Client.Unity;
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
        Container.Bind<Camera>().FromInstance(_mainCamera).AsSingle();
        Container.Bind<CinemachineVirtualCamera>().FromInstance(_virtualCamera).AsSingle();
        Container.BindInterfacesAndSelfTo<CameraManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<CharacterSpawner>().AsSingle();
        InstallInitializationHandling();
        InstallItems();
    }

    private void InstallInitializationHandling()
    {
        Container.BindInstance(_announcer).AsSingle();
        Container.BindInterfacesAndSelfTo<SceneInitializedBaseHandler>().AsSingle();
    }

    private void InstallItems()
    {
        Container.BindInterfacesAndSelfTo<ModuleFactory>().AsSingle().When(x => x.Container == Container);
        Container.BindInterfacesAndSelfTo<ItemFactory>().AsSingle();
    }

}
