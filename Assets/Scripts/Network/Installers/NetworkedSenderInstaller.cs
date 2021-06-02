using Networking.Character;
using System;
using UnityEngine;
using Zenject;

public class NetworkedSenderInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<NetworkedPositionUpdater>().AsSingle();
        Container.BindInterfacesAndSelfTo<InputMessageSender>().AsSingle();
        Container.BindInterfacesAndSelfTo<EquipmentMessageSender>().AsSingle();
        Container.BindInterfacesAndSelfTo<WeaponChangedMessageSender>().AsSingle();
        Container.BindInterfacesAndSelfTo<DamageReceiver>().AsSingle();
        Container.BindInterfacesAndSelfTo<HealthUpdateSender>().AsSingle();
        Container.BindInterfacesAndSelfTo<AssignItemHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<ShootCommandSender>().AsSingle();
    }
}
