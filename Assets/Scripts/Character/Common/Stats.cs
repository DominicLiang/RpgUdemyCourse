
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stats
{
    [SerializeField] private int value;

    [SerializeField] private List<int> modifiers;

    public Stats()
    {
        modifiers = new List<int>();
    }

    public int GetValue()
    {
        var finalValue = value;
        foreach (var modifier in modifiers)
        {
            finalValue += modifier;
        }
        return finalValue;
    }

    public void SetDefaultValue(int value)
    {
        this.value = value;
    }

    public void AddModifier(int modifier)
    {
        modifiers.Add(modifier);
    }

    public void RemoveModifier(int modifier)
    {
        modifiers.Remove(modifier);
    }
}