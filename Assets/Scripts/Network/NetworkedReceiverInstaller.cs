using Zenject;

public class NetworkedReceiverInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MessageReceiver>().AsSingle();
        Container.BindInterfacesAndSelfTo<NetworkedCharacterMovement>().AsSingle();
        Container.BindInterfacesAndSelfTo<NetworkedCharacterShooting>().AsSingle();
    }
}
