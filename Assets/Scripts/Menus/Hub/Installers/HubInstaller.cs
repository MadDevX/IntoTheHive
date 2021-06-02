using Zenject;

public class HubInstaller: MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<HubInitializer>().AsSingle();
    }
}