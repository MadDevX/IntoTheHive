using Zenject;

public class NetworkedSenderInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<PlayerMessageSender>().AsSingle();
    }
}
