using UnityEngine;

public class DisplayInventory : MonoBehaviour
{
    Inventory _inventory;

    void Start()
    {
        gameObject.SetActive(false);
    }

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
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Display();
        }
    }

    void Display()
    {
        UpdateInventory();
        gameObject.SetActive(true);
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
