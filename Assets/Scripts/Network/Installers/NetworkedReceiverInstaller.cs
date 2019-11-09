using Zenject;

public class NetworkedReceiverInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<InputMessageReceiver>().AsSingle();
        Container.BindInterfacesAndSelfTo<EquipmentMessageReceiver>().AsSingle();
        Container.BindInterfacesAndSelfTo<NetworkedCharacterInput>().AsSingle();
        Container.BindInterfacesAndSelfTo<NetworkedCharacterEquipment>().AsSingle();
    }
}
