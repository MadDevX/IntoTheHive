using Zenject;

public class RegistryInstaller: MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<LivingCharactersRegistry>().AsSingle();
    }
}