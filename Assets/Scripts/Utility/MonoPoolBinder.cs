using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public static class MonoPoolBinder
{
    public static void BindMonoContextPool<T, TArgs, TFactory, TPool>(this DiContainer Container, Identifiers id, int size, T prefab, string transformGroupName, BindingCondition cond = null)
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
            FromSubContainerResolve().
            ByNewContextPrefab(prefab).
            UnderTransformGroup(transformGroupName));

        if (cond != null)
        {
            bind.When(cond);
        }
    }

    public static void BindMonoContextPool<T, TArgs, TFactory, TPool>(this DiContainer Container, Identifiers id, int size, T prefab, Transform parentTransform, BindingCondition cond = null)
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
            FromSubContainerResolve().
            ByNewContextPrefab(prefab).
            UnderTransform(parentTransform));

        if (cond != null)
        {
            bind.When(cond);
        }
    }

    public static void BindMonoPrefabPool<T, TArgs, TFactory, TPool>(this DiContainer Container, Identifiers id, int size, T prefab, string transformGroupName, BindingCondition cond = null)
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
            UnderTransformGroup(transformGroupName));

        if (cond != null)
        {
            bind.When(cond);
        }
    }


    public static void BindMonoPrefabPool<T, TArgs, TFactory, TPool>(this DiContainer Container, Identifiers id, int size, T prefab, Transform parentTransform, BindingCondition cond = null)
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
}
