using Zenject;

/// <summary>
/// Installer for a particular type of trigger.
/// Used at the end of level to load hub.
/// </summary>
public class EndLevelTriggerInstaller : MonoInstaller 
{    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<EndLevelTrigger>().AsSingle();
    }
}