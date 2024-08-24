using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpellsManager : MonoBehaviour
{
    [SerializeField] private List<SpellBehavior> spellBehaviors = new();

    private void Awake()
    {
        spellBehaviors.Clear();

        string[] assetNames = AssetDatabase.FindAssets($"t:{typeof(SpellBehavior)}");
        SpellBehavior[] behaviors = new SpellBehavior[assetNames.Length];

        for (int i = 0; i < assetNames.Length; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(assetNames[i]);
            behaviors[i] = AssetDatabase.LoadAssetAtPath<SpellBehavior>(path);
        }

        spellBehaviors.AddRange(behaviors);
    }

    public List<SpellBehavior> GetSpellBehaviors()
    {
        return spellBehaviors;
    }
}
