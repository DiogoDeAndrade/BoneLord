using TMPro;
using UnityEngine;

public class DisplayInventory : UIPanel
{
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI soulText;

    Inventory _inventory;

    public void SetInventory(Inventory inventory)
    {
        if (_inventory)
        {
            _inventory.onChange -= UpdateInventory;
        }
        _inventory = inventory;
        if (_inventory)
        {
            _inventory.onChange += UpdateInventory;
        }
    }

    public override void Open()
    {
        UpdateInventory();

        base.Open();
    }

    void UpdateInventory()
    {
        var items = GetComponentsInChildren<DisplayItem>();
        for (int i = 0; i < items.Length; i++)
        {
            Item item = _inventory.GetItem(i);
            items[i].SetItem(item, _inventory.GetItemCount(item));
        }

        if (goldText) goldText.text = $"x{_inventory.gold}";
        if (soulText) soulText.text = $"x{_inventory.souls}";
    }
}
