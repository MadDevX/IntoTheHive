using Zenject;

public class EncounterInstaller: MonoInstaller
{
    public override void InstallBindings()
    {
        // Scene context
        Container.BindInterfacesAndSelfTo<HostEncounterManager>().AsSingle();
        // Scene context
        Container.BindInterfacesAndSelfTo<HostEncounterEnemyManager>().AsSingle();
        // GameObject context
        Container.BindInterfacesAndSelfTo<RoomEnemyInfoPool>().AsSingle();
    }
}