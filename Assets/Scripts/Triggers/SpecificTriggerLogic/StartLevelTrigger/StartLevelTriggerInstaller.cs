using Zenject;

/// <summary>
/// Installer for a particular type of trigger.
/// Used at the end of hub to load next level.
/// </summary>
public class StartLevelTriggerInstaller: MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<StartLevelTrigger>().AsSingle();
    }
}