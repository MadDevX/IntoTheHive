using Networking.Character;
using Zenject;

public class NetworkedReceiverInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<NetworkedCharacterInput>().AsSingle();
        Container.BindInterfacesAndSelfTo<NetworkedCharacterWeapon>().AsSingle();
        Container.BindInterfacesAndSelfTo<HealthUpdateReceiver>().AsSingle();
    }
}
