using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HUDInstaller : MonoInstaller
{
    [SerializeField] private TMPro.TextMeshProUGUI _healthText;
    [SerializeField] private InventoryWindow _inventoryWindow;
    [SerializeField] private InventorySlot _slotPrefab;
    [SerializeField] private Transform _slotParent;

    public override void InstallBindings()
    {
        InstallComponents();
        InstallLogic();
        InstallSlots();
    }

    private void InstallComponents()
    {
        Container.Bind<TMPro.TextMeshProUGUI>().WithId(Identifiers.Health).FromInstance(_healthText).AsCached();
        Container.Bind<InventoryWindow>().FromInstance(_inventoryWindow).AsSingle();
    }

    private void InstallLogic()
    {
        Container.BindInterfacesAndSelfTo<HealthTracker>().AsSingle();
    }

    private void InstallSlots()
    {
        BindMonoPrefabPool<InventorySlot, ItemInstance, InventorySlot.Factory, InventorySlotPool>
            (Identifiers.InventorySlot, 10, _slotPrefab, _slotParent);
    }

    //TODO: remove this
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            _inventoryWindow.gameObject.SetActive(!_inventoryWindow.gameObject.activeSelf);
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
