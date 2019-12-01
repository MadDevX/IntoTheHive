using System;
using Zenject;

public class LevelGenerationInstaller: MonoInstaller
{
    public override void InstallBindings()
    {
        BindComponents();
        BindFactory();
        BindMessaging();        
    }

    private void BindComponents()
    {
        Container.Bind<Rooms>().AsSingle();
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