using Zenject;

public class NetworkedReceiverInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MessageReceiver>().AsSingle();
        Container.BindInterfacesAndSelfTo<NetworkedCharacterInput>().AsSingle();
        Container.BindInterfacesAndSelfTo<NetworkedCharacterEquipment>().AsSingle();
    }
}
