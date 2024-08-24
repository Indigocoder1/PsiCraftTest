using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParameterItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Transform selectorsParent;
    [SerializeField] private Image defaultSelector;

    private SpellItem spellItem;
    private bool setPreviousSelector;

    public void SetParameter(string name, Type type, bool hasDefault, SpellItem spellItem)
    {
        this.spellItem = spellItem;

        defaultSelector.gameObject.SetActive(hasDefault);

        nameText.text = $"{name} ({type})";

        if(hasDefault)
        {
            StartCoroutine(SelectDefault());
        } 
    }

    public void SelectCell(Image selector)
    {
        foreach (var image in selectorsParent.GetComponentsInChildren<Image>())
        {
            image.color = normalColor;
        }

        selector.color = selectedColor;

        spellItem.SetParamOffset(transform.GetSiblingIndex(), selector.GetComponent<ParameterGridOffset>().GetOffset());
    }

    public void SetSelectedSelector()
    {
        if (spellItem == null) return;

        var offsets = spellItem.GetParamOffsets();
        if (offsets == null) return;

        if (!offsets.TryGetValue(transform.GetSiblingIndex(), out Vector2Int currentOffset)) return;

        var selector = selectorsParent.GetComponentsInChildren<ParameterGridOffset>().FirstOrDefault(i => i.GetOffset() == currentOffset);
        if (selector == null) return;

        SelectCell(selector.GetComponent<Image>());

        setPreviousSelector = true;
    }

    private IEnumerator SelectDefault()
    { 
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        if (setPreviousSelector) yield break;

        SelectCell(defaultSelector);
    }
}
