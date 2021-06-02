using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Zenject;

public class ModuleFactory
{
    public ReadOnlyCollection<short> Ids { get; private set; }
        
    private Dictionary<short, BaseModule> _modules;

    private DiContainer _container;

    public ModuleFactory(DiContainer container)
    {
        _container = container;
        InitDictionary();
        InitIds();
    }

    public IModule Create(short id)
    {
        if(_modules.TryGetValue(id, out var module))
        {
            return module.Clone();
        }
        else
        {
            Debug.LogError($"Module with requested id({id}) does not exist!");
            return null;
        }
    }

    private void InitDictionary()
    {
        _modules = new Dictionary<short, BaseModule>();
        Debug.Log("Initializing module dictionary");
        foreach (Type t in this.GetType().Assembly.GetTypes())
        {
            if(t.GetInterfaces().Contains(typeof(IModule)) && t.IsInterface == false && t.IsAbstract == false)
            {
                Debug.Log($"creating {t.Name}");
                var instance = (BaseModule)Activator.CreateInstance(t);
                if (instance == null) Debug.LogError("Error creating module!");
                else
                {
                    if (_modules.ContainsKey(instance.Id))
                    {
                        _modules.TryGetValue(instance.Id, out var module);
                        Debug.LogError($"Module ID conflict: {instance.GetType().Name}.Id == {module.GetType().Name}.Id");
                    }
                    else
                    {
                        _container.QueueForInject(instance);
                        _modules.Add(instance.Id, instance);
                    }
                }
            }
        }
    }

    private void InitIds()
    {
        Ids = _modules.Keys.ToList().AsReadOnly();
    }
}
