using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "GameResources/UISettings")]
public class UISettingsInstaller : ScriptableObjectInstaller
{
    [SerializeField] private HealthTracker.Settings _healthTrackerSettings;
    [SerializeField] private InventorySlot.Settings _inventorySlotSettings;
    [SerializeField] private ServerInfoTracker.Settings _serverTrackerSettings;
    [SerializeField] private FloatingText.Settings _floatingTextSettings;
    public override void InstallBindings()
    {
        Container.BindInstance(_healthTrackerSettings).AsSingle();
        Container.BindInstance(_inventorySlotSettings).AsSingle();
        Container.BindInstance(_serverTrackerSettings).AsSingle();
        Container.BindInstance(_floatingTextSettings).AsSingle();
    }
}
