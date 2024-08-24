using UnityEngine;

public class TooltipWindow : MonoBehaviour
{
    [SerializeField] private GameObject behaviorItemPrefab;
    [SerializeField] private Transform spawnParent;
    [SerializeField] private SpellsManager spellManager;

    private SpellItem currentSpellItem;

    private void Start()
    {
        foreach (var behavior in spellManager.GetSpellBehaviors())
        {
            var item = Instantiate(behaviorItemPrefab, spawnParent).GetComponent<UIBehaviorItem>();
            item.SetBehaviorItem(behavior);
            item.SetWindow(this);
        }
    }

    public void SetSelectedItem(SpellItem item)
    {
        currentSpellItem = item;
    }

    public void SelectBehavior(UIBehaviorItem behaviorItem)
    {
        currentSpellItem.SetSpellBehavior(behaviorItem.GetSpellBehavior());
    }
}
