using System;
using UnityEngine;
using Zenject;

public class LevelGenerationInstaller: MonoInstaller
{
    [SerializeField] private Rooms _rooms;

    public override void InstallBindings()
    {
        BindComponents();
        BindFactory();
        BindMessaging();        
    }

    private void BindComponents()
    {
        Container.Bind<Rooms>().FromInstance(_rooms).AsSingle();
        Container.BindInterfacesAndSelfTo<LevelSpawner>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelGraphState>().AsSingle();
        Container.BindInterfacesAndSelfTo<BasicLevelGraphGenerator>().AsSingle();
        Container.BindInterfacesAndSelfTo<SpawnParametersGenerator>().AsSingle();
    }

    private void BindFactory()
    {
        Container.BindFactory<RoomSpawnParameters, RoomFacade, RoomFacade.Factory>().FromFactory<RoomFactory>();
    }

    private void BindMessaging()
    {
        Container.BindInterfacesAndSelfTo<LevelGraphMessageReceiver>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelGraphMessageSender>().AsSingle();
    }
}