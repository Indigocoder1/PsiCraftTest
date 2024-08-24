using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }

    [SerializeField] private RectTransform backgroundRect;
    [SerializeField] private TooltipWindow tooltip;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void OpenTooltipWindow(RectTransform target, SpellItem item)
    {
        tooltip.gameObject.SetActive(true);

        var rect = tooltip.GetComponent<RectTransform>();
        Vector2 targetPosition = (Vector2)target.position + (target.sizeDelta / 2) + (rect.sizeDelta / 2);

        if(!RectTransformUtility.RectangleContainsScreenPoint(backgroundRect, targetPosition))
        {
            bool above = targetPosition.y + backgroundRect.rect.height > backgroundRect.rect.height;
            targetPosition.y -= above ? rect.sizeDelta.y : 0;
        }    

        rect.position = targetPosition;
        tooltip.SetSelectedItem(item);
    }

    public void CloseTooltipWindow()
    {
        tooltip.gameObject.SetActive(false);
    }
}
