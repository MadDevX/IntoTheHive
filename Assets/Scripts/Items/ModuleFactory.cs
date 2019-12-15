using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModuleFactory
{
    private Dictionary<int, BaseModule> _modules;

    public ModuleFactory()
    {
        InitDictionary();
    }

    public IModule Create(int id)
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
        _modules = new Dictionary<int, BaseModule>();
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
                        _modules.Add(instance.Id, instance);
                    }
                }
            }
        }
    }
}
