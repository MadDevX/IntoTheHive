using System;
using UnityEngine;
using Zenject;

public class NetworkedSenderInstaller : MonoInstaller
{
    [SerializeField]
    private InputMessageSender.Settings _inputSenderSettings;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<InputMessageSender.Settings>().FromInstance(_inputSenderSettings).AsSingle();
        Container.BindInterfacesAndSelfTo<InputMessageSender>().AsSingle();
        Container.BindInterfacesAndSelfTo<EquipmentMessageSender>().AsSingle();
    }
}
