using Zenject;

public class NetworkedSenderInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<InputMessageSender>().AsSingle();
        Container.BindInterfacesAndSelfTo<EquipmentMessageSender>().AsSingle();
    }
}
