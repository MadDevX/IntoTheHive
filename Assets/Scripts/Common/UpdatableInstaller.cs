using Relays;
using Relays.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UpdatableInstaller : MonoInstaller
{
    [SerializeField] private MonoRelay _relay;

    public override void InstallBindings()
    {
        Container.Bind<IRelay>().FromInstance(_relay).AsSingle();
    }
}
