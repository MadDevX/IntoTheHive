using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IComponent
{
    GameObject GameObject { get; }
    Transform Transform { get; }
}


/// <summary>
/// Used if context-based prefab has no MonoBehaviour derived facade - thus when MonoPoolableMemoryPool is not applicable
/// </summary>
/// <typeparam name="TParam1"></typeparam>
/// <typeparam name="TParam2"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class ContextGameObjectMemoryPool<TParam1, TParam2, TValue>
        : MemoryPool<TParam1, TParam2, TValue>
        where TValue : IComponent, IPoolable<TParam1, TParam2>
{
    Transform _originalParent;

    [Inject]
    public ContextGameObjectMemoryPool()
    {
    }

    protected override void OnCreated(TValue item)
    {
        item.GameObject.SetActive(false);
        _originalParent = item.Transform.parent;
    }

    protected override void OnDestroyed(TValue item)
    {
        GameObject.Destroy(item.GameObject);
    }

    protected override void OnDespawned(TValue item)
    {
        item.OnDespawned();
        item.GameObject.SetActive(false);

        if (item.Transform.parent != _originalParent)
        {
            item.Transform.SetParent(_originalParent, false);
        }
    }

    protected override void Reinitialize(TParam1 p1, TParam2 p2, TValue item)
    {
        item.GameObject.SetActive(true);
        item.OnSpawned(p1, p2);
    }
}
