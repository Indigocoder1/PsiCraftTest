using System;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Spell Behavior")]
public class SpellBehavior : ScriptableObject
{
    public string spellName;
    public Sprite spellIcon;
    public Color iconColor;
    public string methodName = string.Empty;
    public string fullMethodClassName = string.Empty;

    public MethodInfo GetMethodInfo()
    {
        Type type = Type.GetType(fullMethodClassName);
        return type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
    }

    public bool MethodHasReturn()
    {
        return GetMethodInfo().ReturnType != typeof(void);
    }
}
