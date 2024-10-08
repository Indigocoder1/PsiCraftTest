using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CustomEditor(typeof(SpellBehavior))]
public class SpellBehaviorEditor : Editor
{
    public static MethodInfo[] AllSpellMethods 
    { 
        get
        {
            if(_allSpellMethods == null)
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

    private AdvancedStringOptionsDropdown _dropdown;
    private SpellBehavior _spellBehavior;

    private FieldInfo _methodNameProperty;
    private FieldInfo _fullMethodClassNameProperty;

    private void Awake()
    {
        CompilationPipeline.assemblyCompilationFinished += (_, __) =>
        {
            AllSpellMethods = null;
        };
    }

    public override void OnInspectorGUI()
    {
        _spellBehavior = (SpellBehavior)target;

        if(_methodNameProperty == null)
        {
            _methodNameProperty = typeof(SpellBehavior).GetField("methodName", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        if (_fullMethodClassNameProperty == null)
        {
            _fullMethodClassNameProperty = typeof(SpellBehavior).GetField("fullMethodClassName", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("Spell Name", GUILayout.Width(80));
        _spellBehavior.spellName = GUILayout.TextField(_spellBehavior.spellName);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Spell Icon", GUILayout.Width(80));
        _spellBehavior.spellIcon = (Sprite)EditorGUILayout.ObjectField(_spellBehavior.spellIcon, typeof(Sprite), false, null);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Icon Color", GUILayout.Width(80));
        _spellBehavior.iconColor = EditorGUILayout.ColorField(_spellBehavior.iconColor);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Typed Inputs", GUILayout.Width(80));
        _spellBehavior.typedInputs = EditorGUILayout.Toggle(_spellBehavior.typedInputs);
        GUILayout.EndHorizontal();

        string[] methodNames = new string[AllSpellMethods.Length];
        for (int i = 0; i < AllSpellMethods.Length; i++)
        {
            methodNames[i] = AllSpellMethods[i].Name;
        }

        _dropdown = new(methodNames);
        _dropdown.OnOptionSelected += index =>
        {
            _methodNameProperty.SetValue(_spellBehavior, AllSpellMethods[index].Name);
            _fullMethodClassNameProperty.SetValue(_spellBehavior, typeof(AllSpellBehaviors).FullName);
        };

        string methodName = (string)_methodNameProperty.GetValue(_spellBehavior) != string.Empty ? (string)_methodNameProperty.GetValue(_spellBehavior) : "None";

        GUIContent content = new GUIContent()
        {
            text = $"Method: {methodName}"
        };
        GUIStyle style = new();

        if (GUILayout.Button(content))
        {
            Rect rect = GUILayoutUtility.GetRect(content, style);
            rect.size = new Vector2(rect.width * 2, rect.height);
            _dropdown.Show(rect);
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(_spellBehavior);
    }
}

public class AdvancedStringOptionsDropdown : AdvancedDropdown
{
    private string[] _itemNames;

    public event Action<int> OnOptionSelected;

    public AdvancedStringOptionsDropdown(string[] stringOptions) : base(new AdvancedDropdownState())
    {
        _itemNames = stringOptions;
    }

    protected override void ItemSelected(AdvancedDropdownItem item)
    {
        OnOptionSelected?.Invoke(item.id);
    }

    protected override AdvancedDropdownItem BuildRoot()
    {
        var root = new AdvancedDropdownItem("");

        for (int i = 0; i < _itemNames.Length; i++)
        {
            var item = new AdvancedDropdownItem(_itemNames[i])
            {
                id = i
            };

            root.AddChild(item);
        }

        return root;
    }
}