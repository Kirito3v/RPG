using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private int baseValue;

    public List<int> modifiers;

    public int GetValue()
    {
        int finalValue = baseValue;

        foreach (int modifier in modifiers)
            finalValue += modifier;
        
        return finalValue;
    }

    public int SetDefaultValue(int value) => baseValue = value;

    public void AddModifier(int mod) => modifiers.Add(mod);

    public void RemoveModifier(int mod) => modifiers.RemoveAt(mod);
}
