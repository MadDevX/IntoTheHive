using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HUDInstaller : MonoInstaller
{
    [SerializeField] private TMPro.TextMeshProUGUI _healthText;
    [SerializeField] private ItemListWindow _inventoryWindow;
    [SerializeField] private ItemListWindow _equipmentWindow;
    [SerializeField] private InventorySlot _slotPrefab;
    [SerializeField] private Transform _inventorySlotParent;
    [SerializeField] private Transform _equipmentSlotParent;
    [SerializeField] private GameObject _windowParent;
    [SerializeField] private GameObject _overlayParent;
    public override void InstallBindings()
    {
        InstallComponents();
        InstallLogic();
        InstallSlots();
    }

    private void InstallComponents()
    {
        Container.Bind<TMPro.TextMeshProUGUI>().WithId(Identifiers.Health).FromInstance(_healthText).AsCached();
        Container.Bind<GameObject>().WithId(Identifiers.Overlay).FromInstance(_overlayParent).AsCached();
        Container.Bind<ItemListWindow>().WithId(Identifiers.Inventory).FromInstance(_inventoryWindow).AsCached();
        Container.Bind<ItemListWindow>().WithId(Identifiers.Equipment).FromInstance(_equipmentWindow).AsCached();
    }

    private void InstallLogic()
    {
        Container.BindInterfacesAndSelfTo<HealthTracker>().AsSingle();
        Container.BindInterfacesAndSelfTo<UnassignedItems>().AsSingle();
        Container.BindInterfacesAndSelfTo<OverlayManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerDamageTracker>().AsSingle();
    }

    private void InstallSlots()
    {
        Container.BindMonoPrefabPool<InventorySlot, ItemInstance, InventorySlot.Factory, InventorySlotPool>
            (Identifiers.Inventory, 10, _slotPrefab, _inventorySlotParent);
        Container.BindMonoPrefabPool<InventorySlot, ItemInstance, InventorySlot.Factory, InventorySlotPool>
            (Identifiers.Equipment, 10, _slotPrefab, _equipmentSlotParent);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            _windowParent.SetActive(!_windowParent.activeSelf);
        }
    }

    public class InventorySlotPool : MonoPoolableMemoryPool<ItemInstance, IMemoryPool, InventorySlot>
    {
    }
}
