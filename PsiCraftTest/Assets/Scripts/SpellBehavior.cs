using System;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Spell Behavior")]
public class SpellBehavior : ScriptableObject
{
    public string spellName;
    public Sprite spellIcon;
    public Color iconColor;
    public MethodInfo methodInfo;
    public bool typedInputs;

    private string methodName = string.Empty;
    private string fullMethodClassName = string.Empty;

    public MethodInfo GetMethodInfo()
    {
        if (methodInfo != null) return methodInfo;

        Type type = Type.GetType(fullMethodClassName);
        methodInfo = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);

        return methodInfo;
    }

    public bool MethodHasReturn()
    {
        return GetMethodInfo().ReturnType != typeof(void);
    }
}
