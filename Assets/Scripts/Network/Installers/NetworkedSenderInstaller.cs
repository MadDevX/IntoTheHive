﻿using System;
using UnityEngine;
using Zenject;

public class NetworkedSenderInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        
        Container.BindInterfacesAndSelfTo<InputMessageSender>().AsSingle();
        Container.BindInterfacesAndSelfTo<NetworkedPositionUpdater>().AsSingle();
        Container.BindInterfacesAndSelfTo<EquipmentMessageSender>().AsSingle();
    }
}