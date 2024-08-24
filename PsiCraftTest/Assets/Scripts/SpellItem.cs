using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellItem : MonoBehaviour
{
    [SerializeField] private Image icon;

    private SpellBehavior behavior;
    private Dictionary<int, Vector2Int> parameterGridOffsets = new();
    private Dictionary<int, float> parameterValues = new();

    public void OpenSelectorMenu()
    {
        TooltipManager.Instance.OpenTooltipWindow(GetComponent<RectTransform>(), this);
    }

    private void Update()
    {
        HandleParameterSetting();
        HandleSpellSelection();
    }

    private void HandleParameterSetting()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        if (!MouseOverItem()) return;

        if (behavior == null) return;

        ParameterWindow.Instance.SetTargetMethod(behavior.GetMethodInfo(), this);
    }

    private void HandleSpellSelection()
    {
        if (!Input.GetMouseButtonDown(1)) return;

        if (!MouseOverItem()) return;

        OpenSelectorMenu();
    }

    private bool MouseOverItem()
    {
        PointerEventData data = new(EventSystem.current);
        data.position = Input.mousePosition;

        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, raycastResults);

        bool mouseOver = raycastResults.Any(i =>
        {
            var spellItem = i.gameObject.GetComponentInParent<SpellItem>();
            return spellItem != null && spellItem == this;
        });

        return mouseOver;
    }

    public SpellBehavior GetSpellBehavior()
    {
        return behavior;
    }

    public void SetSpellBehavior(SpellBehavior behavior)
    {
        this.behavior = behavior;
        icon.sprite = behavior.spellIcon;
        icon.color = behavior.iconColor;
        parameterGridOffsets = new();

        for (int i = 0; i < behavior.GetMethodInfo().GetParameters().Length; i++)
        {
            parameterGridOffsets.Add(i, Vector2Int.zero);
            parameterValues.Add(i, 0);
        }
    }

    public void SetParamOffset(int index, Vector2Int offset)
    {
        if (!parameterGridOffsets.ContainsKey(index))
        {
            parameterGridOffsets.Add(index, offset);
            return;
        }

        parameterGridOffsets[index] = offset;
    }

    public void SetParameterValue(int index, float value)
    {
        if (!parameterValues.ContainsKey(index))
        {
            parameterValues.Add(index, value);
            return;
        }

        parameterValues[index] = value;
    }

    public Dictionary<int, Vector2Int> GetParamOffsets()
    {
        return parameterGridOffsets;
    }

    public Dictionary<int, float> GetParameterValues()
    {
        return parameterValues;
    }
}
