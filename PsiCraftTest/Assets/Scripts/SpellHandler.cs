using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpellHandler : MonoBehaviour
{
    [SerializeField] private GameObject spellItemPrefab;
    [SerializeField] private Transform spellItemsParent;
    [SerializeField] private int itemsWidth;
    [SerializeField] private int itemsHeight;
    [SerializeField] private RectTransform craftingGrid;
    [SerializeField] private RectTransform spellsHolder;
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private SpellsManager spellsManager;

    private List<SpellItem> spellItems = new();

    private void Start()
    {
        gridLayout.spacing = new Vector2(10, 10);
        float width = (itemsWidth + 1)* (gridLayout.cellSize.x + gridLayout.spacing.x + gridLayout.padding.left + gridLayout.padding.right);
        float height = (itemsHeight + 1) * (gridLayout.cellSize.y + gridLayout.spacing.y + gridLayout.padding.top + gridLayout.padding.bottom);
        width += spellsHolder.sizeDelta.x;
        height += spellsHolder.sizeDelta.y;

        Vector2 size = new(width, height);

        craftingGrid.sizeDelta = size;

        for (int i = 0; i < itemsHeight * itemsHeight; i++)
        {
            var spellItem = Instantiate(spellItemPrefab, spellItemsParent).GetComponent<SpellItem>();

            spellItems.Add(spellItem);
        }
    }

    public SpellItem GetItemAtPosition(Vector2Int gridPosition)
    {
        int index = (gridPosition.x - 1) + (gridPosition.y - 1) * itemsHeight;
        if (index < 0 || index > spellItems.Count - 1) return null;

        return spellItems[index];
    }

    public Vector2Int GetItemPosition(SpellItem item)
    {
        int childIndex = item.transform.GetSiblingIndex();
        int x = childIndex % itemsHeight;
        int y = (childIndex - x) / itemsWidth;

        return new Vector2Int(x + 1, y + 1);
    }

    public void RunSpells()
    {
        List<SpellItem> tricks = new();
        foreach (var item in spellItems)
        {
            if (item.GetSpellBehavior() == null) continue;

            if(item.GetSpellBehavior().GetMethodInfo().ReturnType == typeof(void))
            {
                tricks.Add(item);
            }
        }

        foreach (var spell in tricks)
        {
            TryRunSpell(spell);
        }
    }

    private object TryRunSpell(SpellItem spell)
    {
        object[] parameters = new object[spell.GetSpellBehavior().GetMethodInfo().GetParameters().Length];
        var spellMethodInfo = spell.GetSpellBehavior().GetMethodInfo();
        Dictionary<int, Vector2Int> offsets = spell.GetParamOffsets();

        Vector2Int spellPos = GetItemPosition(spell);

        for (int i = 0; i < parameters.Length; i++)
        {
            var param = spellMethodInfo.GetParameters()[i];
            if (offsets[i] == Vector2Int.zero && param.DefaultValue != DBNull.Value)
            {
                parameters[i] = param.DefaultValue;
                continue;
            }

            if(spell.GetSpellBehavior().typedInputs)
            {
                parameters[i] = spell.GetParameterValues()[i];
                continue;
            }

            if (offsets[i] == Vector2Int.zero)
            {
                Debug.LogError($"No input tile set on position {spellPos}");
                return null;
            }

            var adjacentSpell = GetItemAtPosition(spellPos + offsets[i]);
            if (adjacentSpell == null)
            {
                return null;
            }

            var method = adjacentSpell.GetSpellBehavior().GetMethodInfo();
            if(method.GetParameters().Length > 0)
            {
                parameters[i] = TryRunSpell(adjacentSpell);
                continue;
            }

            parameters[i] = method.Invoke(null, null);
        }

        return spell.GetSpellBehavior().GetMethodInfo().Invoke(null, parameters);
    }
}
