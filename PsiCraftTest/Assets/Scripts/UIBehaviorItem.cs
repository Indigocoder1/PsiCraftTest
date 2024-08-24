using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviorItem : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI itemText;

    private SpellBehavior spellBehavior;
    private TooltipWindow tooltipWindow;

    public void SetBehaviorItem(SpellBehavior behavior)
    {
        spellBehavior = behavior;

        iconImage.sprite = behavior.spellIcon;
        iconImage.color = behavior.iconColor;
        itemText.text = behavior.spellName;
    }

    public void SetWindow(TooltipWindow window)
    {
        tooltipWindow = window;
    }

    public SpellBehavior GetSpellBehavior()
    {
        return spellBehavior;
    }

    public void SelectBehavior()
    {
        tooltipWindow.SelectBehavior(this);
    }
}
