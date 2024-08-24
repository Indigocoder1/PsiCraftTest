using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Spell Behavior")]
public class SpellBehavior : ScriptableObject
{
    public string spellName;
    public Sprite spellIcon;
    public Color iconColor;
    public int methodIndex;

    public MethodInfo GetMethodInfo()
    {
        return SpellsManager.AllSpellMethods[methodIndex];
    }

    public bool MethodHasReturn()
    {
        return GetMethodInfo().ReturnType != typeof(void);
    }
}
