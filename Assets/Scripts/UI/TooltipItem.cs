using TMPro;
using UnityEngine;

public class TooltipItem : Tooltip
{
    [SerializeField] TextMeshProUGUI text;

    public void SetItem(Item item)
    {
        if (item != null)
        {
            text.text = item.displayName;
            lastUpdated = Time.time;
            Open();
        }
        else
        {
            text.text = "";
            Close();
        }
    }
}
