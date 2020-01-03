using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierType
{
    PercentageBase,
    Flat,
    PercentageCumulative
}

public class Modifier : IComparable
{
    public ModifierType type;
    public float value;

    public Modifier(ModifierType type, float value)
    {
        this.type = type;
        this.value = value;
    }

    /// <summary>
    /// Calculates and returns value after applying modifier bonus to <paramref name="currentValue"/>
    /// </summary>
    /// <param name="currentValue">Current value to which bonus will be added</param>
    /// <param name="referenceValue">Reference value for bonus calculations</param>
    /// <returns>Value after applying bonus to <paramref name="currentValue"/></returns>
    public float ApplyModifier(float currentValue, float referenceValue)
    {
        //TODO: if another MultiplierType will be added - just change this to polymorphism 
        if (type == ModifierType.Flat)
        {
            return currentValue + value;
        }
        else if (type == ModifierType.PercentageBase)
        {
            return currentValue + referenceValue * (value - 1.0f); //-1, so that multipliers like 1.05 will act like 105% of normal damage dealt, and modifiers like -1.0f will negate damage
        }
        else if ( type == ModifierType.PercentageCumulative)
        {
            return currentValue * value; //like case above, but cumulative percentage does not take referenceValue into consideration, operating always on latest value
        }
        else
        {
            throw new ArgumentException("ModifierType handler is not implemented");
        }
    }

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;
        var mod = obj as Modifier;  
        if(mod != null)
        {
            return type.CompareTo(mod.type);
        }
        else
        {
            throw new ArgumentException("Object is not a Modifier");
        }
    }
}

public class ModifierList
{
    private List<Modifier> _modifiers;

    public ModifierList()
    {
        _modifiers = new List<Modifier>();
    }

    public void Add(Modifier mod)
    {
        _modifiers.Add(mod);
        _modifiers.Sort((x1, x2) => x1.type.CompareTo(x2.type));
    }

    public void Remove(Modifier mod)
    {
        _modifiers.Remove(mod);
    }

    /// <summary>
    /// Calculates final value using <paramref name="baseValue"/> and currently active multipliers
    /// </summary>
    /// <param name="baseValue"></param>
    /// <returns></returns>
    public float CalculateFinalValue(float baseValue)
    {
        var result = baseValue;
        var referenceValue = baseValue;
        var lastIdx = _modifiers.Count - 1;
        for (int i = 0; i < _modifiers.Count; i++)
        {
            result = _modifiers[i].ApplyModifier(result, referenceValue);
            if (_modifiers[i].type != _modifiers[Mathf.Min(lastIdx, i + 1)].type)
            {
                referenceValue = result;
            }
        }
        return result;
    }
}
