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

    public override void InstallBindings()
    {
        InstallComponents();
        InstallLogic();
        InstallSlots();
    }

    private void InstallComponents()
    {
        Container.Bind<TMPro.TextMeshProUGUI>().WithId(Identifiers.Health).FromInstance(_healthText).AsCached();
        Container.Bind<ItemListWindow>().WithId(Identifiers.Inventory).FromInstance(_inventoryWindow).AsCached();
        Container.Bind<ItemListWindow>().WithId(Identifiers.Equipment).FromInstance(_equipmentWindow).AsCached();
    }

    private void InstallLogic()
    {
        Container.BindInterfacesAndSelfTo<HealthTracker>().AsSingle();
        Container.BindInterfacesAndSelfTo<UnassignedItems>().AsSingle();
    }

    private void InstallSlots()
    {
        BindMonoPrefabPool<InventorySlot, ItemInstance, InventorySlot.Factory, InventorySlotPool>
            (Identifiers.Inventory, 10, _slotPrefab, _inventorySlotParent);
        BindMonoPrefabPool<InventorySlot, ItemInstance, InventorySlot.Factory, InventorySlotPool>
            (Identifiers.Equipment, 10, _slotPrefab, _equipmentSlotParent);
    }

    //TODO: remove this
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            _windowParent.SetActive(!_windowParent.activeSelf);
        }
    }



    private void BindMonoPrefabPool<T, TArgs, TFactory, TPool>(Identifiers id, int size, T prefab, Transform parentTransform, BindingCondition cond = null)
    where T : MonoBehaviour, IPoolable<TArgs, IMemoryPool>
    where TFactory : PlaceholderFactory<TArgs, T>
    where TPool : MonoPoolableMemoryPool<TArgs, IMemoryPool, T>
    {
        var bind =
        Container.BindFactory<TArgs, T, TFactory>().
            WithId(id).
            FromPoolableMemoryPool<TArgs, T, TPool>
            (x => x.WithInitialSize(size).
            ExpandByDoubling().
            FromComponentInNewPrefab(prefab).
            UnderTransform(parentTransform));

        if (cond != null)
        {
            bind.When(cond);
        }
    }

    public class InventorySlotPool : MonoPoolableMemoryPool<ItemInstance, IMemoryPool, InventorySlot>
    {
    }
}
