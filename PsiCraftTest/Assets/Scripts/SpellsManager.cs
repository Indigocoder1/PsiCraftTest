using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Compilation;
using UnityEngine;

public class SpellsManager : MonoBehaviour
{
    public static MethodInfo[] AllSpellMethods
    {
        get
        {
            if (_allSpellMethods == null)
            {
                _allSpellMethods = typeof(AllSpellBehaviors).GetMethods(BindingFlags.Static | BindingFlags.Public);
            }

            return _allSpellMethods;
        }
        private set
        {
            _allSpellMethods = typeof(AllSpellBehaviors).GetMethods(BindingFlags.Static | BindingFlags.Public);
        }
    }

    private static MethodInfo[] _allSpellMethods;

    [SerializeField] private List<SpellBehavior> spellBehaviors;

    private void Awake()
    {
        CompilationPipeline.assemblyCompilationFinished += (_, __) =>
        {
            AllSpellMethods = null;
        };
    }

    public List<SpellBehavior> GetSpellBehaviors()
    {
        return spellBehaviors;
    }
}
