using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "GameResources/UISettings")]
public class UISettingsInstaller : ScriptableObjectInstaller
{
    [SerializeField] private HealthTracker.Settings _healthTrackerSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(_healthTrackerSettings).AsSingle();
    }
}
