using Networking.Items;
using Relays;
using Relays.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PickupInstaller : MonoInstaller
{
    [SerializeField] private ItemPickup _facade;
    [SerializeField] private MonoRelay _relay;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ItemPickup>().FromInstance(_facade).AsSingle();
        Container.BindInterfacesAndSelfTo<NetId>().AsSingle();
        Container.BindInterfacesAndSelfTo<ItemRespawnable>().AsSingle();
        Container.BindInterfacesAndSelfTo<PickupCollisionHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<DespawnMessageHandler>().AsSingle();
        Container.Bind<IRelay>().FromInstance(_relay).AsSingle();
    }
}
