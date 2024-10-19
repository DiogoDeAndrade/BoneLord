using UnityEngine;

public class DisplayInventory : UIPanel
{
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

    public void ToggleDisplay()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
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
    }
}
