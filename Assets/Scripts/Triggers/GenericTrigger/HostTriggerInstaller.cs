using Relays;
using Relays.Internal;
using UnityEngine;
using Zenject;

/// <summary>
/// Generic Installer for triggers.
/// Triggers are concrete only on host, triggers on client side only have dummy triggers.
/// </summary>
public class HostTriggerInstaller: MonoInstaller 
{
    [SerializeField]
    private BoxCollider2D _trigger;

    [SerializeField]
    private TriggerRelay _relay;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TriggerRelay>().FromInstance(_relay);
        Container.BindInstance(_trigger);
        Container.BindInterfacesAndSelfTo<HostTriggerCounter>().AsSingle();
    }
}