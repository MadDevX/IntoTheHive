using System;
using UnityEngine;
using Zenject;

public class LevelGenerationInstaller: MonoInstaller
{
    [SerializeField] private Rooms _rooms;
    [SerializeField] private Triggers _triggers;
    [SerializeField] private Doors _doors;

    public override void InstallBindings()
    {
        
        InstallComponents();
        InstallFactory();
        InstallMessaging();
        InstallDoorPlacing();
        InstallCommunication();
    }

    private void InstallCommunication()
    {
        Container.BindInterfacesAndSelfTo<LevelGraphMessageSender>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelGraphState>().AsSingle();
        Container.BindInterfacesAndSelfTo<BasicLevelGraphGenerator>().AsSingle();
    }

    private void InstallComponents()
    {
        //TODO MG : make some mechanic that allows for interscene communication without. Maybe a signal bus?
        //Also move classes from ProjectNetworkInstaller here 
        Container.Bind<Rooms>().FromInstance(_rooms).AsSingle();
        Container.Bind<Triggers>().FromInstance(_triggers).AsSingle();
        Container.Bind<Doors>().FromInstance(_doors).AsSingle();
        Container.BindInterfacesAndSelfTo<LevelSpawner>().AsSingle();
        Container.BindInterfacesAndSelfTo<SpawnParametersGenerator>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameplayInitializer>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelSpawnParameters>().AsSingle();
    }

    private void InstallFactory()
    {
        Container.BindFactory<RoomSpawnParameters, RoomFacade, RoomFacade.Factory>().FromFactory<RoomFactory>();
        Container.BindFactory<DoorSpawnParameters, DoorFacade, DoorFacade.Factory>().FromFactory<DoorFactory>();
        Container.BindFactory<TriggerSpawnParameters, TriggerFacade, TriggerFacade.Factory>().FromFactory<TriggerFactory>();
    }

    private void InstallDoorPlacing()
    {
        Container.BindInterfacesAndSelfTo<DoorManager>().AsSingle();
    }

    private void InstallMessaging()
    {
        Container.BindInterfacesAndSelfTo<LevelGraphMessageReceiver>().AsSingle();
        Container.BindInterfacesAndSelfTo<HostDoorManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<ClientDoorManager>().AsSingle();
    }
}