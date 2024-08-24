using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class ParameterItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TMP_InputField paramInputField;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Transform selectorsParent;
    [SerializeField] private Image defaultSelector;

    private SpellItem spellItem;
    private bool setPreviousSelector;
    private float typedParamValue;

    private Coroutine previousCoroutine;

    public void SetParameter(string name, Type type, bool hasDefault, SpellItem spellItem)
    {
        this.spellItem = spellItem;

        defaultSelector.gameObject.SetActive(hasDefault);

        nameText.text = $"{name} ({type})";

        if(hasDefault)
        {
            StartCoroutine(SelectDefault());
        }

        bool hasTypedInputs = spellItem.GetSpellBehavior().typedInputs;
        selectorsParent.gameObject.SetActive(!hasTypedInputs);
        paramInputField.gameObject.SetActive(hasTypedInputs);
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

    public void SetupItemDelayed()
    {
        if (spellItem == null) return;

        SetPreviousSelection();
        SetInputValue();
    }

    private void SetPreviousSelection()
    {
        var offsets = spellItem.GetParamOffsets();
        if (offsets == null) return;

        if (!offsets.TryGetValue(transform.GetSiblingIndex(), out Vector2Int currentOffset)) return;

        var selector = selectorsParent.GetComponentsInChildren<ParameterGridOffset>().FirstOrDefault(i => i.GetOffset() == currentOffset);
        if (selector == null) return;

        SelectCell(selector.GetComponent<Image>());

        setPreviousSelector = true;
    }

    private void SetInputValue()
    {
        var paramValues = spellItem.GetParameterValues();

        if (!paramValues.TryGetValue(transform.GetSiblingIndex(), out float value)) return;

        paramInputField.text = value.ToString();
    }

    public void OnParamInputChanged(string input)
    {
        if(previousCoroutine != null)
        {
            StopCoroutine(previousCoroutine);
        }

        previousCoroutine = StartCoroutine(CacheParamValDelayed(input));
    }

    private IEnumerator CacheParamValDelayed(string value)
    {
        yield return new WaitForSeconds(0.2f);

        typedParamValue = float.Parse(value);
        spellItem.SetParameterValue(transform.GetSiblingIndex(), typedParamValue);
    }

    private IEnumerator SelectDefault()
    { 
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        if (setPreviousSelector) yield break;

        SelectCell(defaultSelector);
    }
}
